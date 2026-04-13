namespace MellonBank.Infrastructure.Options
{
    public sealed class FixerOptions
    {
        public const string SectionName = "Fixer";
        public string BaseUrl { get; set; } = "https://data.fixer.io/api/";
        public string ApiKey { get; set; } = string.Empty;
    }
}
