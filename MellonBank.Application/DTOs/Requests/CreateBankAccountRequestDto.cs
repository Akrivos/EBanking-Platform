using MellonBank.Domain.Enums;

namespace MellonBank.Application.DTOs.Requests
{
    public record CreateBankAccountRequestDto(
        string CustomerAfm,
        string AccountNumber,
        decimal InitialBalance,
        CurrencyType Currency,
        string Branch,
        AccountType AccountType
    );
}
