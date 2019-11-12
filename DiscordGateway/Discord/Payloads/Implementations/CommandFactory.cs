using DiscordGateway.Discord.Payloads.Abstractions;
using DiscordGateway.Discord.Payloads.Implementations.Commands;
using Microsoft.Extensions.Configuration;

namespace DiscordGateway.Discord.Payloads.Implementations
{
    public class CommandFactory : ICommandFactory
    {
        private readonly string _token;
        public CommandFactory(IConfiguration cfg)
        {
            _token = cfg.GetSection(Bot.BotConstants.BOT_SECRETS)[Bot.BotConstants.TOKEN];
        }

        public ICommand<IPayload> CreateCommand<IPayload>(Constants.OpCode opCode, IPayload payload = default)
        {
            switch(opCode)
            {
                case Constants.OpCode.Identify:
                    var id = new Identify(_token);
                    return new Command<Identify> { Op = opCode, Data = id };
                case Constants.OpCode.Heartbeat:
                    break;
                case Constants.OpCode.RequestGuildMembers:
                    break;
                case Constants.OpCode.Resume:
                    break;
                case Constants.OpCode.StatusUpdate:
                    return new Command<UpdateStatus>();
                case Constants.OpCode.VoiceStateUpdate:
                    return new Command();
                default:
                    return new Command<IPayload>();
            }
            return new Command();
        }
    }
}
