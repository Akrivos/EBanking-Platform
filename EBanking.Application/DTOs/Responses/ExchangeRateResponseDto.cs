using EBanking.Domain.Enums;

namespace EBanking.Application.DTOs.Responses
{
    public sealed record ExchangeRateResponseDto(
        CurrencyType BaseCurrency,
        CurrencyType TargetCurrency,
        decimal Rate,
        DateTime RetrievedAtUtc
    );
}
