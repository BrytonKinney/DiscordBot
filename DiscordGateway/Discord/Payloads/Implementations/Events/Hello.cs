using DiscordGateway.Discord.Payloads.Abstractions;
using System.Text.Json.Serialization;

namespace DiscordGateway.Discord.Payloads.Implementations.Events
{
    public class Hello : IPayload
    {
        [JsonPropertyName("heartbeat_interval")]
        public int HeartbeatInterval { get; set; }

        [JsonPropertyName("_trace")]
        public object Trace { get; set; }
    }
}
