using MellonBank.Domain.Enums;


namespace MellonBank.Application.DTOs.Responses
{
    public record CustomerAccountDetailsResponseDto(
        Guid Id,
        string AccountNumber,
        decimal Balance,
        CurrencyType Currency,
        string Branch,
        AccountType AccountType
    );
}
