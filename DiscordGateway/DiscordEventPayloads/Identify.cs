using DiscordGateway.DiscordObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DiscordGateway.DiscordEventPayloads
{
    public class Identify
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("compress")]
        public bool CompressionEnabled { get; set; }

        [JsonPropertyName("large_threshold")]
        public int GuildMemberThreshold { get; set; }

        [JsonPropertyName("shard")]
        public int[]? GuildShards { get; set; }

        [JsonPropertyName("guild_subscriptions")]
        public bool SubscribeToGuildEvents { get; set; }

        [JsonPropertyName("presence")]
        public UpdateStatus Status { get; set; }

        [JsonPropertyName("properties")]
        public ConnectionProperties Properties { get; set; }
    }
}
