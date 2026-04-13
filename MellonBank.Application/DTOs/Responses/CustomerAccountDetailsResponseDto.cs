using MellonBank.Domain.Enums;


namespace MellonBank.Application.DTOs.Responses
{
    public record CustomerAccountDetailsResponseDto(
        string AccountNumber,
        decimal Balance,
        CurrencyType Currency,
        string Branch,
        AccountType AccountType
    );
}
