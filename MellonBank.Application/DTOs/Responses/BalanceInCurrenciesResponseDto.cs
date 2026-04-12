namespace MellonBank.Application.DTOs.Responses
{
    public record BalanceInCurrenciesResponseDto(
        string AccountNumber,
        decimal BalanceEuro,
        decimal EuroToUsdRate,
        decimal BalanceUsd
    );
}
