using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.DTOs.Responses;

namespace MellonBank.Application.Interfaces.Services
{
    public interface IAccountManagementService
    {
        Task<Guid> CreateAccountAsync(CreateBankAccountRequestDto request, CancellationToken ct = default);
        Task UpdateAccountAsync(string accountNumber, UpdateBankAccountRequestDto request, CancellationToken ct = default);
        Task DeleteAccountAsync(string accountNumber, CancellationToken ct = default);
        Task<AccountDetailsResponseDto?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
    }
}
