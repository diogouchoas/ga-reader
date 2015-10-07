using CommandLine;

namespace ga_reader.Infrastructure
{
    [Verb("q", HelpText = "Query")]
    public class QueryOptions
    {
        [Option("onlytotals", HelpText = "Mostrar somente o total.", Default = false)]
        public bool OnlyTotals { get; set; }

        [Option('t', "tableid", Required = true, HelpText = "TableId. Ex: ga:21810642")]
        public string TableId { get; set; }

        [Option('m', "metrics", Required = true, HelpText = "Métricas. Ex: rt:activeUsers ")]
        public string Metrics { get; set; }

        [Option('d', "dimensions", HelpText = "Dimensões. Ex: rt:city")]
        public string Dimensions { get; set; }
    }

    [Verb("r", HelpText = "Reset")]
    public class ResetOptions
    {
    }

    [Verb("a", HelpText = "Authorize")]
    public class AuthorizeOptions
    {
    }
}