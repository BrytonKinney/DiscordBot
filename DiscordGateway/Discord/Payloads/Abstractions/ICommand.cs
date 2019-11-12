using DiscordGateway.Discord.Constants;
using DiscordGateway.Discord.Payloads.Implementations.Commands;
using System.Text.Json.Serialization;

namespace DiscordGateway.Discord.Payloads.Abstractions
{
    public interface ICommand<T>
    {
        /// <summary>
        /// The opcode.
        /// </summary>
        public OpCode Op { get; }

        /// <summary>
        /// The payload.
        /// </summary>
        public T Data { get; }
    }
}
