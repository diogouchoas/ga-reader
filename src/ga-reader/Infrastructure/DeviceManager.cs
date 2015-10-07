using ga_reader.Infrastructure.Client;
using ga_reader.Infrastructure.Data.ApiResponses;
using ga_reader.Infrastructure.Storage;
using System;

namespace ga_reader.Infrastructure
{
    public class DeviceManager
    {
        private readonly Store _storage;
        private readonly GoogleApiClient _apiClient;

        public DeviceManager(Store storage, GoogleApiClient apiClient)
        {
            if (storage == null)
                throw new ArgumentNullException("storage");

            if (apiClient == null)
                throw new ArgumentNullException("apiClient");

            _storage = storage;
            _apiClient = apiClient;
        }

        public DeviceAuthorizationResponse Authorize()
        {
            var authorizationResponse = _apiClient.AuthorizeDevice(Constants.ClientId);
            if (authorizationResponse == null)
                throw new Exception("Invalid device authorization response.");

            _storage.DeviceAuthorizationFile.Save(authorizationResponse).GetAwaiter().GetResult();
            return authorizationResponse;
        }

        public string GetDeviceCode()
        {
            var deviceAuthorization = _storage.DeviceAuthorizationFile.Recover();
            if (deviceAuthorization == null)
                throw new Exception("Device Authorization not found. Run -a command first.");

            return deviceAuthorization.device_code;
        }
    }
}