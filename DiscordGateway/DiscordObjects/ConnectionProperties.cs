using System.Text.Json.Serialization;

namespace DiscordGateway.DiscordObjects
{
    public class ConnectionProperties
    {
        [JsonPropertyName("$os")]
        public string OperatingSystem { get; set; }
        [JsonPropertyName("$browser")]
        public string Browser { get; set; }
        [JsonPropertyName("$device")]
        public string Device { get; set; }
    }
}
