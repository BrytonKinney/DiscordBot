using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DiscordGateway.DiscordObjects.GuildActivity
{
    public class Party
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("size")]
        public int[]? PartySize { get; set; }
    }
}
