using DiscordGateway.DiscordEventPayloads;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiscordGateway
{
    public class DiscordSocketClient : IDiscordSocketClient
    {
        private ClientWebSocket _client;
        private string _baseEndpoint;
        private const int MAX_BUFFER = 512;
        private readonly ILogger<DiscordSocketClient> _logger;
        private Pipe _payloadPipe;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="discordEndpoint"></param>
        public DiscordSocketClient(ILogger<DiscordSocketClient> logger, string discordEndpoint)
        {
            _client = new ClientWebSocket();
            _baseEndpoint = discordEndpoint;
            _logger = logger;
            _payloadPipe = new Pipe();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DiscordSocketClient> ConnectAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            await _client.ConnectAsync(new Uri(_baseEndpoint), cancellationToken);
            await HandleResultAsync(cancellationToken);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StreamResultToPipeAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            while (true)
            {
                Memory<byte> memoryBuffer = _payloadPipe.Writer.GetMemory(MAX_BUFFER);
                try
                {
                    var val = await _client.ReceiveAsync(memoryBuffer, cancellationToken);
                    if (val.Count == 0)
                        break;
                    _payloadPipe.Writer.Advance(val.Count + 1);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while streaming the results to the pipeline.");
                    break;
                }
                var flush = await _payloadPipe.Writer.FlushAsync();
                if (flush.IsCompleted)
                    break;
            }
            await _payloadPipe.Writer.CompleteAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ReadFromPipeAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            while (true)
            {
                try
                {
                    ReadResult result = await _payloadPipe.Reader.ReadAsync();
                    ReadOnlySequence<byte> resultBuffer = result.Buffer;
                    SequencePosition? pos;
                    do
                    {
                        pos = resultBuffer.PositionOf((byte)'\0');
                        if (pos != null)
                        {
                            await ProcessPayload(resultBuffer.Slice(0, pos.Value));
                            resultBuffer = resultBuffer.Slice(resultBuffer.GetPosition(1, pos.Value));
                        }
                    } while (pos != null);
                    _payloadPipe.Reader.AdvanceTo(resultBuffer.Start, resultBuffer.End);
                    if (result.IsCompleted)
                        break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing data from pipe.");
                }
            }
            await _payloadPipe.Reader.CompleteAsync();
        }

        public async Task ProcessPayload(ReadOnlySequence<byte> buffer)
        {
            try
            {
                _logger.LogInformation("Received data: {0}", Encoding.UTF8.GetString(buffer.FirstSpan));
                var payload = JsonSerializer.Deserialize<DiscordPayload>(buffer.FirstSpan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payload.");
            }
        }

        public async Task SendAsync(Memory<byte> buffer, System.Threading.CancellationToken cancellationToken = default)
        {

        }

        /// <summary>
        /// Processes results received from the websocket via a <see cref="System.IO.Pipelines.Pipe"/>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DiscordSocketClient> HandleResultAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            await Task.WhenAll(StreamResultToPipeAsync(cancellationToken), ReadFromPipeAsync(cancellationToken));
            return this;
        }

        internal class DiscordPayload
        {
            [JsonPropertyName("op")]
            public OpCode Op { get; set; }

            [JsonConverter(typeof(DiscordEventPayloadConverter))]
            [JsonPropertyName("d")]
            public object Event { get; set; }
        }

        public class DiscordEventPayloadConverter : JsonConverter<BaseEvent>
        {
            private System.Text.StringBuilder _stringBuilder;

            public DiscordEventPayloadConverter()
            {
                _stringBuilder = new StringBuilder();
            }

            public override bool CanConvert(Type typeToConvert)
            {
                return typeToConvert == typeof(System.String);
            }

            public override BaseEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        internal enum OpCode
        {
            Dispatch,
            Heartbeat,
            Identify,
            StatusUpdate,
            VoiceStateUpdate,
            Resume,
            Reconnect,
            RequestGuildMembers,
            InvalidSession,
            Hello,
            HeartbeatAck
        }
    }
}
