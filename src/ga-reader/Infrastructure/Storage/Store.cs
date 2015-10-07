using ga_reader.Infrastructure.Data.ApiResponses;

namespace ga_reader.Infrastructure.Storage
{
    public class Store
    {
        public readonly FileStorage<DeviceAuthorizationResponse> DeviceAuthorizationFile
            = new FileStorage<DeviceAuthorizationResponse>("device-auth.json");

        public readonly FileStorage<TokenResponse> TokenFile
            = new FileStorage<TokenResponse>("token.json");

        public readonly FileStorage<RefreshTokenResponse> RefreshTokenFile
            = new FileStorage<RefreshTokenResponse>("refresh_token.json");

        public void Wipe()
        {
            DeviceAuthorizationFile.Clear();
            TokenFile.Clear();
            RefreshTokenFile.Clear();
        }
    }
}