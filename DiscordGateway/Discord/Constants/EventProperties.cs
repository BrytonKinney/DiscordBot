using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordGateway.Discord.Constants
{
    public static class EventProperties
    {
        // Heartbeat
        public const string HEARTBEAT_INTERVAL = "heartbeat_interval";

        // Update Status
        public const string STATUS = "status";
        public const string IDLE_TIME = "since";
        public const string ACTIVITY = "game";
        public const string IS_AFK = "afk";

        // Connection properties
        public const string OPERATING_SYSTEM = "$os";
        public const string BROWSER = "$browser";
        public const string DEVICE = "$device";

        // Guild Activity
        public const string NAME = "name";
        public const string ACTIVITY_TYPE = "type";
        public const string STREAM_URL = "url";
        public const string ACTIVITY_TIMESTAMPS = "timestamps";
    }
}
