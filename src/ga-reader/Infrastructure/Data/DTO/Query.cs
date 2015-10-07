namespace ga_reader.Infrastructure.Data.DTO
{
    public class Query
    {
        public string ids { get; set; }
        public string[] metrics { get; set; }
        public int maxresults { get; set; }
    }
}