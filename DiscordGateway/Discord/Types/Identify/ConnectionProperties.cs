using System.Text.Json.Serialization;

namespace DiscordGateway.Discord.Types.Identify
{
    public class ConnectionProperties
    {
        [JsonPropertyName(Constants.EventProperties.OPERATING_SYSTEM)]
        public string OperatingSystem { get; set; }

        [JsonPropertyName(Constants.EventProperties.BROWSER)]
        public string Browser { get; set; }

        [JsonPropertyName(Constants.EventProperties.DEVICE)]
        public string Device { get; set; }
    }
}
