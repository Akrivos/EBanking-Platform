namespace MellonBank.Application.DTOs.Responses
{
    public sealed record BalanceInCurrenciesResponseDto(
        string AccountNumber,
        decimal BalanceEuro,
        decimal EuroToUsdRate,
        decimal BalanceUsd
    );
}
