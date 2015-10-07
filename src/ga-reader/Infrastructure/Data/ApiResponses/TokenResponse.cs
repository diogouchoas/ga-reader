namespace ga_reader.Infrastructure.Data.ApiResponses
{
    public class TokenResponse
    {
        public string error { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
    }
}