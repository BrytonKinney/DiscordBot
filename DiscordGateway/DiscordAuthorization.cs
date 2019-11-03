using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGateway
{
    public class DiscordAuthorization : IDiscordAuthorization
    {
        private string _clientId;
        private string _clientSecret;
        private string _redirectUri;
        private string _authorizationToken;
        private List<string> _scopes;
        public DiscordAuthorization(string clientId, string clientSecret, string redirectUri)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _redirectUri = redirectUri;
            //_scopes = new List<string>()
            //{
            //    "bot",
            //    "identify",
            //    "connections",
            //    "email",
            //    "guilds",
            //    "guilds.join",
            //    "gdm.join",
            //    "rpc",
            //    "messages.read",
            //    "rpc.api",
            //    "rpc.notifications.read",
            //    "webhook.incoming",
            //    "applications.builds.upload",
            //    "applications.builds.read",
            //    "applications.store.update",
            //    "applications.entitlements",
            //    "activities.read",
            //    "activities.write",
            //    "relationships.read"
            //};
        }

        public async Task<string> GetAuthorizationTokenAsync()
        {
            if (string.IsNullOrWhiteSpace(_authorizationToken))
            {
                // should only be ran once every ten minutes or so, so w/e
                // see: https://tools.ietf.org/html/rfc6749#section-3.2.1
                using (var httpClient = new HttpClient())
                {
                    //var headers = new List<KeyValuePair<string, string>>
                    //{
                    //    new KeyValuePair<string, string>( "response_type", "code"),
                    //    new KeyValuePair<string, string>( "client_id", _clientId),
                    //    new KeyValuePair<string, string>("redirect_uri", _redirectUri),
                    //    new KeyValuePair<string, string>("scopes",  string.Join("%20", _scopes)),
                    //    new KeyValuePair<string, string>("permissions", "8")
                    //};
                    var queryString = string.Format("response_type=code&client_id={0}&redirect_uri={1}&scope={2}&permissions=8", _clientId, WebUtility.UrlEncode(_redirectUri), "bot");
                    var authUrl = string.Format("https://discordapp.com/api/oauth2/authorize?{0}", queryString);
                    var resp = await httpClient.GetAsync(new Uri(authUrl));
                    var respContent = await resp.Content.ReadAsStringAsync();
                }
            }
            return _authorizationToken;
        }
    }
}
