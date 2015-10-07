using ga_reader.Infrastructure.Client;
using ga_reader.Infrastructure.Data.ApiResponses;
using ga_reader.Infrastructure.Storage;
using System;

namespace ga_reader.Infrastructure
{
    public class TokenManager
    {
        private readonly Store _storage;
        private readonly GoogleApiClient _apiClient;
        private readonly string _deviceCode;

        public TokenManager(string deviceCode, Store storage, GoogleApiClient apiClient)
        {
            if (deviceCode == null)
                throw new ArgumentNullException("deviceCode");

            if (storage == null)
                throw new ArgumentNullException("storage");

            if (apiClient == null)
                throw new ArgumentNullException("apiClient");

            _deviceCode = deviceCode;
            _storage = storage;
            _apiClient = apiClient;
        }

        public void CreateRefreshToken()
        {
            var currentToken =
                _storage.TokenFile.Recover() ?? CreateToken();

            if (currentToken == null)
                throw new Exception("Cannot recover or generate the token.");

            var refreshTokenResponse =
                _apiClient.GetRefreshToken(
                    Constants.ClientId,
                    Constants.ClientSecret,
                    currentToken.refresh_token);

            if (refreshTokenResponse == null)
                throw new Exception("Fatal: Can't find a proper refresh token.");

            _storage.RefreshTokenFile.Save(refreshTokenResponse).GetAwaiter().GetResult();
        }

        public string GetAccessToken()
        {
            var currentRefreshToken = _storage.RefreshTokenFile.Recover();
            if (currentRefreshToken != null)
                return currentRefreshToken.access_token;

            var currentToken = _storage.TokenFile.Recover() ?? CreateToken();
            if (currentToken == null)
                throw new Exception("Could not retrieve or create the access token.");

            return currentToken.access_token;
        }

        private TokenResponse CreateToken()
        {
            var token =
                _apiClient.GetToken(Constants.ClientId, Constants.ClientSecret, _deviceCode);

            if (token == null || token.error != null)
            {
                _storage.Wipe();
                throw new Exception(
                    string.Format("Could not get the token with device_code {0}.\nToken error: {1}", _deviceCode,
                        token == null ? string.Empty : token.error));
            }

            _storage.TokenFile.Save(token).GetAwaiter().GetResult();
            return token;
        }
    }
}