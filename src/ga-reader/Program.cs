using CommandLine;
using ga_reader.Exceptions;
using ga_reader.Infrastructure;
using ga_reader.Infrastructure.Client;
using ga_reader.Infrastructure.Storage;
using Newtonsoft.Json;
using Polly;
using System;

namespace ga_reader
{
    internal class Program
    {
        private static readonly Store Storage = new Store();
        private static readonly GoogleApiClient ApiClient = new GoogleApiClient();

        private static void Main(string[] args)
        {
            if (string.IsNullOrWhiteSpace(Constants.ClientId) ||
                string.IsNullOrWhiteSpace(Constants.ClientSecret))
                throw new Exception("Invalid configuration for client_id or client_secret.");

            try
            {
                Parser.Default.ParseArguments<QueryOptions, ResetOptions, AuthorizeOptions>(args)
                    .MapResult(
                        (QueryOptions opts) => RunQuery(opts),
                        (ResetOptions opts) => RunReset(opts),
                        (AuthorizeOptions opts) => RunAuthorize(opts),
                        errs => false);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        private static bool RunAuthorize(AuthorizeOptions opts)
        {
            var deviceManager = new DeviceManager(Storage, ApiClient);
            var authorizationResponse = deviceManager.Authorize();

            Console.WriteLine("Open the browser and navigate to: {0}?user_code={1}" +
                              "\nWhen asked for the code, enter: {1}", authorizationResponse.verification_url, authorizationResponse.user_code);

            return true;
        }

        private static bool RunReset(ResetOptions opts)
        {
            Storage.Wipe();
            return true;
        }

        private static bool RunQuery(QueryOptions opts)
        {
            var deviceManager = new DeviceManager(Storage, ApiClient);

            var deviceCode = deviceManager.GetDeviceCode();
            if (deviceCode == null)
                return false;

            var data = AuthenticateAndRetrieveData(deviceCode, opts);

            Console.WriteLine(JsonConvert.SerializeObject(opts.OnlyTotals ? data.totalsForAllResults : data, Formatting.Indented));
            return true;
        }

        private static dynamic AuthenticateAndRetrieveData(string deviceCode, QueryOptions queryOptions)
        {
            var tokenManager = new TokenManager(deviceCode, Storage, ApiClient);

            dynamic realTimeData = null;

            var policy =
                Policy
                    .Handle<UnauthorizedException>()
                    .Retry(1, (exception, retryCount) =>
                    {
                        tokenManager.CreateRefreshToken();
                        realTimeData = ApiClient.GetRealTimeData(queryOptions, tokenManager.GetAccessToken());
                    });

            policy.Execute(() =>
            {
                realTimeData = ApiClient.GetRealTimeData(queryOptions, tokenManager.GetAccessToken());
            });

            return realTimeData;
        }
    }
}