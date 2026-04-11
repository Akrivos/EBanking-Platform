using MellonBank.Domain.Enums;

namespace MellonBank.Application.DTOs.Requests
{
    public record UpdateBankAccountRequestDto(string? Branch, AccountType? AccountType);
}
