using EBanking.Domain.Enums;

namespace EBanking.Application.DTOs.Requests
{
    public sealed record UpdateBankAccountRequestDto(string? Branch, AccountType? AccountType);
}
