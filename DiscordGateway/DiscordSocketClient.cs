using DiscordGateway.Discord.Payloads.Abstractions;
using DiscordGateway.Discord.Payloads.Implementations.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscordGateway
{
    public class DiscordSocketClient : IDiscordSocketClient
    {
        private ClientWebSocket _client;
        private const string _baseEndpoint = "wss://gateway.discord.gg/?v=6&encoding=json";
        private const int MAX_BUFFER = 512;
        private readonly ILogger<DiscordSocketClient> _logger;
        private readonly ICommandFactory _cmdFactory;
        //private readonly IDiscordAuthorization _authorizer;
        private Pipe _payloadPipe;
        private int _heartbeatInterval;
        private string _token;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="discordEndpoint"></param>
        public DiscordSocketClient(ILogger<DiscordSocketClient> logger, IConfiguration configuration, ICommandFactory cmdFactory)
        {
            _client = new ClientWebSocket();
            _logger = logger;
            _payloadPipe = new Pipe();
            _token = configuration.GetSection(Bot.BotConstants.BOT_SECRETS)[Bot.BotConstants.TOKEN];
            _cmdFactory = cmdFactory;
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
            return this;
        }

        private void UpdateHeartbeatInterval(int heartbeatInterval)
        {
            _heartbeatInterval = heartbeatInterval;
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
        }

        public async Task ProcessPayload(ReadOnlySequence<byte> buffer, System.Threading.CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Received data: {0}", Encoding.UTF8.GetString(buffer.FirstSpan));
                var payload = JsonSerializer.Deserialize<Command>(buffer.FirstSpan);
                switch(payload.Op)
                {
                    case Discord.Constants.OpCode.Hello:
                        await SendAsync(_cmdFactory.CreateCommand<Identify>(Discord.Constants.OpCode.Identify), cancellationToken);
                        break;
                    case Discord.Constants.OpCode.Heartbeat:
                        break;
                    case Discord.Constants.OpCode.HeartbeatAck:
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payload.");
            }
        }

        public async Task SendAsync<T>(ICommand<T> eventPayload, System.Threading.CancellationToken cancellationToken = default)
        {
            var serializedPayload = JsonSerializer.SerializeToUtf8Bytes(eventPayload);
            _logger.LogInformation("Sending payload: {0}", System.Text.Encoding.UTF8.GetString(serializedPayload));
            await _client.SendAsync(serializedPayload, WebSocketMessageType.Text, true, cancellationToken); 
        }

        /// <summary>
        /// Processes results received from the websocket via a <see cref="System.IO.Pipelines.Pipe"/>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DiscordSocketClient> HandleResultAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            await Task.WhenAll(StreamResultToPipeAsync(cancellationToken), ReadFromPipeAsync(cancellationToken));
            _payloadPipe.Reset();
            return this;
        }
    }
}
