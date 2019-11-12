using DiscordGateway.Discord.Constants;
using DiscordGateway.Discord.Payloads.Abstractions;
using DiscordGateway.Discord.Types.Identify;
using System;
using System.Text.Json.Serialization;

namespace DiscordGateway.Discord.Payloads.Implementations.Commands
{
    public class Identify : IPayload
    {
        public Identify() { }
        public Identify(string token)
        {
            Token = token;
        }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("compress")]
        public bool CompressionEnabled { get; set; } = false;

        [JsonPropertyName("large_threshold")]
        public int GuildMemberThreshold { get; set; } = 50;

        [JsonPropertyName("shard")]
        public int[] GuildShards { get; set; } = new int[]{ 0, 1 };

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
            Browser = "SBDUBot V2",
            Device = "SBDUBot V2",
            OperatingSystem = System.Environment.OSVersion.Platform.ToString()
        };
    }
}
