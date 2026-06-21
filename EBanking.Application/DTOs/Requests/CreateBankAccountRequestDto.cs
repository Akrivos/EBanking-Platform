using EBanking.Domain.Enums;

namespace EBanking.Application.DTOs.Requests
{
    public sealed record CreateBankAccountRequestDto(
        string CustomerAfm,
        decimal InitialBalance,
        CurrencyType Currency,
        string Branch,
        AccountType AccountType
    );
}
