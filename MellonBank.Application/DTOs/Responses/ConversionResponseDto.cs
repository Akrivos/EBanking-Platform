using System.Text.Json.Serialization;

namespace MellonBank.Application.DTOs.Responses
{
    public sealed class ConversionResponseDto
    {
        [JsonPropertyName("conversion_rate")]
        public decimal ConversionRate { get; set; }

        [JsonPropertyName("conversion_result")]
        public decimal ConversionResult { get; set; }
    }
}