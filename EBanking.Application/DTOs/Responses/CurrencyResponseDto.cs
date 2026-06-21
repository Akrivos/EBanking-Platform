namespace EBanking.Application.DTOs.Responses
{
    public sealed record CurrencyResponseDto(
        decimal Aud,
        decimal Chf,
        decimal Gbp,
        decimal Usd
    );
}
