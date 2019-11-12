using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DiscordGateway.Discord.Types.Identify
{
    public class UpdateStatus
    {
        [JsonPropertyName(Constants.EventProperties.IDLE_TIME)]
        public int? IdleTime { get; set; }

        [JsonPropertyName(Constants.EventProperties.STATUS)]
        public string Status { get; set; }

        [JsonPropertyName(Constants.EventProperties.IS_AFK)]
        public bool IsAfk { get; set; }

        [JsonPropertyName(Constants.EventProperties.ACTIVITY)]
        public GuildActivity.Activity Activity { get; set; }
    }
}
