using System.Text.Json.Serialization;

namespace DiscordGateway.DiscordEventPayloads
{
    public class Hello : BaseEvent
    {
        [JsonPropertyName("heartbeat_interval")]
        public int HeartbeatInterval { get; set; }

        [JsonPropertyName("_trace")]
        public object Trace { get; set; }
    }
}
