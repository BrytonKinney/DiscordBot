using DiscordGateway.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DiscordGateway.DiscordEventPayloads
{
    public class DispatchPayload
    {
        [JsonPropertyName("op")]
        public OpCode Op { get; set; }

        [JsonPropertyName("d")]
        public object Event { get; set; }

        [JsonPropertyName("s")]
        public int SequenceNumber { get; set; }

        [JsonPropertyName("t")]
        public string EventName { get; set; }
    }
}
