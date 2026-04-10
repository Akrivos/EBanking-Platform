using MellonBank.Domain.Enums;

namespace MellonBank.Application.DTOs.Responses
{
    public record BankAccountDto(
        string AccountNumber,
        decimal Balance,
        CurrencyType Currency,
        bool IsActive
    );
}
