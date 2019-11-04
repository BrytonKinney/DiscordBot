using DiscordGateway.DiscordObjects;
using System.Text.Json.Serialization;

namespace DiscordGateway.DiscordEventPayloads
{
    public class Identify
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("compress")]
        public bool CompressionEnabled { get; set; } = false;

        [JsonPropertyName("large_threshold")]
        public int GuildMemberThreshold { get; set; } = 50;

        [JsonPropertyName("shard")]
        public int[]? GuildShards { get; set; } = null;

        [JsonPropertyName("guild_subscriptions")]
        public bool SubscribeToGuildEvents { get; set; } = true;

        [JsonPropertyName("presence")]
        public UpdateStatus Status { get; set; } = new UpdateStatus()
        {
            IdleTime = 0,
            IsAfk = false,
            Status = "Solving Captchas"
        };

        [JsonPropertyName("properties")]
        public ConnectionProperties Properties { get; set; } = new ConnectionProperties()
        {
            Browser = "DiscordBot V2",
            Device = "DiscordBot V2",
            OperatingSystem = "Windows 10"
        };
    }
}
