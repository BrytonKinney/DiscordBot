using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordGateway.Discord.Payloads.Abstractions
{
    public interface ICommandFactory
    {
        ICommand<T> CreateCommand<T>(Constants.OpCode opCode, IPayload payload = default);
    }
}
