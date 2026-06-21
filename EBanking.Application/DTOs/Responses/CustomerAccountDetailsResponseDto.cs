using EBanking.Domain.Enums;


namespace EBanking.Application.DTOs.Responses
{
    public sealed record CustomerAccountDetailsResponseDto(
        string AccountNumber,
        decimal Balance,
        CurrencyType Currency,
        string Branch,
        AccountType AccountType
    );
}
