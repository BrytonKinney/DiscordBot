using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DiscordGateway;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordBot
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IDiscordSocketClient _discSock;
        public Worker(ILogger<Worker> logger, IDiscordSocketClient discSock)
        {
            _logger = logger;
            _discSock = discSock;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _discSock.ConnectAsync(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await _discSock.HandleResultAsync(stoppingToken);
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
        }
    }
}
