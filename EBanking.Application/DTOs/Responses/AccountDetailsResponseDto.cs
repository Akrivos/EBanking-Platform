using EBanking.Domain.Enums;

namespace EBanking.Application.DTOs.Responses
{
    public sealed record AccountDetailsResponseDto(
        Guid Id,
        string AccountNumber,
        decimal Balance,
        CurrencyType Currency,
        string Branch,
        AccountType AccountType,
        string? CustomerAfm = null
    );
}
