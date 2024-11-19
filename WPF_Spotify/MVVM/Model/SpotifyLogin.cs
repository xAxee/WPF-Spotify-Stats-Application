using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace WPF_Spotify.MVVM.Model
{
    public interface ISpotifyLogin
    {
        public void login();
    }
    public class SpotifyLogin : ISpotifyLogin
    {
        private static EmbedIOAuthServer _server;
        private string _clientId = "a945fd0c49674bf5bdc9ea62e888bb1b";
        private string _secretId = "c041292172014d4b943b0057de1f1246";

        public SpotifyLogin()
        {
            login();
        }

        public void login()
        {
            // Create server
            _server = new EmbedIOAuthServer(new Uri("http://localhost:5000/callback"), 5000);
            _server.Start();

            _server.AuthorizationCodeReceived += OnAuthorizationCodeReceived;
            _server.ErrorReceived += OnErrorReceived;

            var request = new LoginRequest(_server.BaseUri, _clientId, LoginRequest.ResponseType.Code)
            {
                Scope = new List<string> { Scopes.UserReadEmail, Scopes.UserReadRecentlyPlayed, Scopes.UserReadCurrentlyPlaying, Scopes.UserModifyPlaybackState, Scopes.Streaming, Scopes.UserTopRead }
            };
            BrowserUtil.Open(request.ToUri());
        }

        private async Task OnAuthorizationCodeReceived(object sender, AuthorizationCodeResponse response)
        {
            await _server.Stop();

            var config = SpotifyClientConfig.CreateDefault();
            var tokenResponse = await new OAuthClient(config).RequestToken(
              new AuthorizationCodeTokenRequest(
                _clientId, _secretId, response.Code, new Uri("http://localhost:5000/callback")
              )
            );
            Token.access_token = tokenResponse.AccessToken;
            Token.expires_in = tokenResponse.ExpiresIn;
            Token.token_type = tokenResponse.TokenType;
        }

        private async Task OnErrorReceived(object sender, string error, string state)
        {
            Console.WriteLine($"Aborting authorization, error received: {error}");
            await _server.Stop();
        }
    }
}