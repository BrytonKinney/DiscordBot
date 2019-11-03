using DiscordGateway.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DiscordGateway.DiscordObjects.GuildActivity
{
    public class Activity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public ActivityTypes ActivityType { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
