using MellonBank.Domain.Enums;

namespace MellonBank.Application.DTOs.Requests
{
    public sealed record UpdateBankAccountRequestDto(string? Branch, AccountType? AccountType);
}
