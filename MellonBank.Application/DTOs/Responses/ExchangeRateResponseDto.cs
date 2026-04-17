using MellonBank.Domain.Enums;

namespace MellonBank.Application.DTOs.Responses
{
    public sealed record ExchangeRateResponseDto(
        CurrencyType BaseCurrency,
        CurrencyType TargetCurrency,
        decimal Rate,
        DateTime RetrievedAtUtc
    );
}
