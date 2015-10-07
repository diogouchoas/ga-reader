namespace ga_reader.Infrastructure.Data.ApiResponses
{
    public class RefreshTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}