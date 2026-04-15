namespace MellonBank.Application.DTOs.Responses
{
    public record CurrencyResponseDto(
        decimal Aud,
        decimal Chf,
        decimal Gbp,
        decimal Usd
    );
}
