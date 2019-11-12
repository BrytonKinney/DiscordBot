using DiscordGateway.Discord.Constants;
using DiscordGateway.Discord.Payloads.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DiscordGateway.Discord.Payloads.Implementations.Commands
{
    public class Command<T> : ICommand<T>
    {
        [JsonPropertyName(Constants.PayloadProperties.OP)]
        public OpCode Op { get; set; }

        [JsonPropertyName(Constants.PayloadProperties.DATA)]
        public T Data { get; set; }
    }

    public class Command : ICommand<IPayload>
    {
        [JsonPropertyName(Constants.PayloadProperties.OP)]
        public OpCode Op { get; set; }

        [JsonPropertyName(Constants.PayloadProperties.DATA)]
        public Payload Data { get; set; }
    }
}
