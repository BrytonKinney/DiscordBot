using DiscordGateway.DiscordEventPayloads;
using Microsoft.Extensions.Configuration;
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
        private const string _baseEndpoint = "wss://gateway.discord.gg/?v=6&encoding=json";
        private const int MAX_BUFFER = 512;
        private readonly ILogger<DiscordSocketClient> _logger;
        //private readonly IDiscordAuthorization _authorizer;
        private Pipe _payloadPipe;
        private int _heartbeatInterval;
        private string _token;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="discordEndpoint"></param>
        public DiscordSocketClient(ILogger<DiscordSocketClient> logger, IConfiguration configuration)//, IDiscordAuthorization authorizer)
        {
            _client = new ClientWebSocket();
           //_authorizer = authorizer;
            _logger = logger;
            _payloadPipe = new Pipe();
            _token = configuration.GetSection("BotSecrets")["Token"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DiscordSocketClient> ConnectAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            //await _authorizer.GetAuthorizationTokenAsync();
            await _client.ConnectAsync(new Uri(_baseEndpoint), cancellationToken);
            await HandleResultAsync(cancellationToken);
            return this;
        }

        private void UpdateHeartbeatInterval(int heartbeatInterval)
        {
            _heartbeatInterval = heartbeatInterval;
            //Task.
        }

        private async Task SendIdentify(System.Threading.CancellationToken cancellationToken = default)
        {
            var identify = new Identify()
            {
                CompressionEnabled = false,
                GuildMemberThreshold = 50,
                Token = _token,
                Properties = new DiscordObjects.ConnectionProperties()
                {
                    Browser = "SBDUBot V2",
                    Device = "SBDUBot V2",
                    OperatingSystem = "Windows ME"
                }
            };
            var payload = new DispatchPayload { Event = identify, EventName = "IDENTIFY", Op = Constants.OpCode.Identify, SequenceNumber = 1 };
            var jsonPayload = JsonSerializer.SerializeToUtf8Bytes(payload);
            _logger.LogInformation("Sending Identify: {0}", System.Text.Encoding.UTF8.GetString(jsonPayload));
            await _client.SendAsync(jsonPayload, WebSocketMessageType.Text, true, cancellationToken);
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
                            await ProcessPayload(resultBuffer.Slice(0, pos.Value), cancellationToken);
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

        public async Task ProcessPayload(ReadOnlySequence<byte> buffer, System.Threading.CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Received data: {0}", Encoding.UTF8.GetString(buffer.FirstSpan));
                var payload = JsonSerializer.Deserialize<BasePayload>(buffer.FirstSpan);
                switch(payload.Op)
                {
                    case Constants.OpCode.Hello:
                        UpdateHeartbeatInterval(payload.Event.GetProperty(Constants.EventProperties.HEARTBEAT_INTERVAL).GetInt32());
                        await SendIdentify(cancellationToken);
                        break;
                    case Constants.OpCode.Heartbeat:
                        break;
                    case Constants.OpCode.HeartbeatAck:
                        break;
                    default: break;
                }
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
    }
}
