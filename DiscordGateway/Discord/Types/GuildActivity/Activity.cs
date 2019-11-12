using DiscordGateway.Discord.Constants;
using System.Text.Json.Serialization;

namespace DiscordGateway.Discord.Types.GuildActivity
{
    public class Activity
    {
        [JsonPropertyName(Constants.EventProperties.NAME)]
        public string Name { get; set; }

        [JsonPropertyName(Constants.EventProperties.ACTIVITY_TYPE)]
        public ActivityTypes ActivityType { get; set; }

        [JsonPropertyName(Constants.EventProperties.STREAM_URL)]
        public string Url { get; set; }
    }
}
