using System.Text.Json.Serialization;

namespace DiscordGateway.DiscordEventPayloads
{
    public class HeartbeatAck : BaseEvent
    {
        [JsonPropertyName("heartbeat_interval")]
        public int HeartbeatInterval { get; set; }
    }
}
