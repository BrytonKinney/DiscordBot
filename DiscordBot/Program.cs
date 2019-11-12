using DiscordGateway;
using DiscordGateway.Discord.Payloads.Abstractions;
using DiscordGateway.Discord.Payloads.Implementations;
using Microsoft.Extensions.Configuration;
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
                    //services.AddSingleton<IDiscordAuthorization, DiscordAuthorization>((provider) =>
                    //{
                    //    var cfg = provider.GetService<IConfiguration>();
                    //    var clientSecrets = cfg.GetSection("ClientSecrets");
                    //    return new DiscordAuthorization(
                    //        clientSecrets.GetValue<string>("ClientId"),
                    //        clientSecrets.GetValue<string>("ClientSecret"),
                    //        clientSecrets.GetValue<string>("RedirectUri")
                    //    );
                    //});
                    services.AddSingleton<ICommandFactory, CommandFactory>();
                    services.AddSingleton<IDiscordSocketClient, DiscordSocketClient>();
                });
    }
}
