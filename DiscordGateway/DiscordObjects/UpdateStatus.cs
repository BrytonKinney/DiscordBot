using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DiscordGateway.DiscordObjects
{
    public class UpdateStatus
    {
        [JsonPropertyName("since")]
        public int? IdleTime { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("afk")]
        public bool IsAfk { get; set; }
    }
}
