using System.Text.Json.Serialization;

namespace DiscordGateway.DiscordObjects.GuildActivity
{
    public class Timestamps
    {
        [JsonPropertyName("start")]
        public int? StartTime { get; set; }

        [JsonPropertyName("end")]
        public int? EndingTime { get; set; }
    }
}
