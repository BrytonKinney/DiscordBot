using DiscordGateway.Constants;
using DiscordGateway.Json;
using System.Text.Json.Serialization;

namespace DiscordGateway.DiscordEventPayloads
{
    [JsonConverter(typeof(PayloadSerializer))]
    public class BasePayload
    {
        [JsonPropertyName("op")]
        public OpCode Op { get; set; }

        [JsonPropertyName("d")]
        public object Event { get; set; }
    }
}
