using ga_reader.Exceptions;
using ga_reader.Infrastructure.Data.ApiResponses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ga_reader.Infrastructure.Client
{
    public class GoogleApiClient
    {
        public DeviceAuthorizationResponse AuthorizeDevice(string clientId)
        {
            var scopes = new List<string>
            {
                "https://www.googleapis.com/auth/analytics",
                "https://www.googleapis.com/auth/analytics.readonly"
            };

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("scope", string.Join(" ", scopes))
            });

            using (var client = new HttpClient())
            {
                var result =
                    client.PostAsync("https://accounts.google.com/o/oauth2/device/code", content)
                        .GetAwaiter()
                        .GetResult();

                if (result.StatusCode == HttpStatusCode.BadRequest)
                    throw new Exception("AuthorizeDevice: Bad Request");

                var json =
                    result.Content.ReadAsStringAsync()
                        .GetAwaiter()
                        .GetResult();

                return JsonConvert.DeserializeObject<DeviceAuthorizationResponse>(json);
            }
        }

        public TokenResponse GetToken(string clientId, string clientSecret, string deviceCode)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("code", deviceCode),
                new KeyValuePair<string, string>("grant_type", "http://oauth.net/grant_type/device/1.0")
            });

            using (var client = new HttpClient())
            {
                var result =
                    client.PostAsync("https://www.googleapis.com/oauth2/v3/token", content)
                        .GetAwaiter()
                        .GetResult();

                if (result.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedException();

                if (result.StatusCode == HttpStatusCode.BadRequest)
                    throw new Exception("GetToken: Bad Request");

                var json =
                    result.Content.ReadAsStringAsync()
                        .GetAwaiter()
                        .GetResult();

                return JsonConvert.DeserializeObject<TokenResponse>(json);
            }
        }

        public RefreshTokenResponse GetRefreshToken(string clientId, string clientSecret, string refreshToken)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("grant_type", "refresh_token")
            });

            using (var client = new HttpClient())
            {
                var result =
                    client.PostAsync("https://www.googleapis.com/oauth2/v3/token", content)
                        .GetAwaiter()
                        .GetResult();

                if (result.StatusCode == HttpStatusCode.BadRequest)
                    throw new Exception("GetRefreshToken: Bad Request");

                var json =
                    result.Content.ReadAsStringAsync()
                        .GetAwaiter()
                        .GetResult();

                return JsonConvert.DeserializeObject<RefreshTokenResponse>(json);
            }
        }

        public dynamic GetRealTimeData(QueryOptions queryOptions, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var url = queryOptions.Dimensions == null
                    ? string.Format(
                        "https://content.googleapis.com/analytics/v3/data/realtime?ids={0}&metrics={1}",
                        queryOptions.TableId, queryOptions.Metrics)
                    : string.Format(
                        "https://content.googleapis.com/analytics/v3/data/realtime?ids={0}&metrics={1}&dimensions={2}",
                        queryOptions.TableId, queryOptions.Metrics, queryOptions.Dimensions);

                var result =
                    client.GetAsync(url).GetAwaiter().GetResult();

                switch (result.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new UnauthorizedException();
                    case HttpStatusCode.BadRequest:
                        throw new Exception("GetRealTimeData: Bad Request");
                }

                var json =
                    result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                return JsonConvert.DeserializeObject(json);
            }
        }
    }
}