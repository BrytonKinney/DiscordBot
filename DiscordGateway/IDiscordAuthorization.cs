using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGateway
{
    public interface IDiscordAuthorization
    {
        Task<string> GetAuthorizationTokenAsync();
    }
}
