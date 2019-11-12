using System.Text.Json.Serialization;

namespace DiscordGateway.Discord.Types.GuildActivity
{
    public class Timestamps
    {
        [JsonPropertyName("start")]
        public int? StartTime { get; set; }

        [JsonPropertyName("end")]
        public int? EndingTime { get; set; }
    }
}
