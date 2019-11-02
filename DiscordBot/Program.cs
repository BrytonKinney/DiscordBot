using DiscordGateway;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<IDiscordSocketClient, DiscordSocketClient>((provider) =>
                    {
                        return new DiscordSocketClient(provider.GetService<ILogger<DiscordSocketClient>>(), "wss://gateway.discord.gg/?v=6&encoding=json");
                    });
                });
    }
}
