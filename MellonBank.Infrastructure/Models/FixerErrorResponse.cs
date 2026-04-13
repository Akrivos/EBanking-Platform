using System.Text.Json.Serialization;

namespace MellonBank.Infrastructure.Models
{
    public sealed class FixerErrorResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("info")]
        public string Info { get; set; } = string.Empty;
    }
}
