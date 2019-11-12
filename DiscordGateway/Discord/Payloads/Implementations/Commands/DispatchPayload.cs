using DiscordGateway.Discord.Payloads.Abstractions;
using System.Text.Json.Serialization;

namespace DiscordGateway.Discord.Payloads.Implementations.Commands
{
    public struct DispatchPayload : ICommand<Payload>
    {
        [JsonPropertyName(Discord.Constants.PayloadProperties.OP)]
        public Discord.Constants.OpCode Op { get => Discord.Constants.OpCode.Dispatch; }

        [JsonPropertyName(Discord.Constants.PayloadProperties.DATA)]
        public Payload Data { get; set; }

        [JsonPropertyName(Discord.Constants.PayloadProperties.OP)]
        public int SequenceNumber { get; set; }

        [JsonPropertyName(Discord.Constants.PayloadProperties.EVENT_NAME)]
        public string EventName { get; set; }
    }
}
