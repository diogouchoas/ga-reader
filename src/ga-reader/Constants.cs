using System.Configuration;

namespace ga_reader
{
    public static class Constants
    {
        public static readonly string ClientId = ConfigurationManager.AppSettings.Get("client_id");
        public static readonly string ClientSecret = ConfigurationManager.AppSettings.Get("client_secret");
    }
}