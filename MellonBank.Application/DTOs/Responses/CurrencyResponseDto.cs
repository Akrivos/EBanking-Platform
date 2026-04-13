namespace MellonBank.Application.DTOs.Responses
{
    public record CurrencyResponseDto(
        decimal aud,
        decimal chf,
        decimal gbp,
        decimal usd
    );
}