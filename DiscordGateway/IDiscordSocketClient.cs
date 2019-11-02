using System;
using System.Threading.Tasks;

namespace DiscordGateway
{
    public interface IDiscordSocketClient
    {
        Task<DiscordSocketClient> ConnectAsync(System.Threading.CancellationToken cancellationToken = default);
        Task SendAsync(Memory<byte> buffer, System.Threading.CancellationToken cancellationToken = default);
        Task StreamResultToPipeAsync(System.Threading.CancellationToken cancellationToken = default);
        Task ReadFromPipeAsync(System.Threading.CancellationToken cancellationToken = default);
        Task<DiscordSocketClient> HandleResultAsync(System.Threading.CancellationToken cancellationToken = default);
    }
}
