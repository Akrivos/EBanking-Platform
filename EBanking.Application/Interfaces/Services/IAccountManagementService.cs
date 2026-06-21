using EBanking.Application.Common.Models;
using EBanking.Application.DTOs.Requests;
using EBanking.Application.DTOs.Responses;

namespace EBanking.Application.Interfaces.Services
{
    public interface IAccountManagementService
    {
        Task<IEnumerable<AccountDetailsResponseDto>> GetAllAsync(CancellationToken ct = default);
        Task<Result<Guid>> CreateAccountAsync(CreateBankAccountRequestDto request, CancellationToken ct = default);
        Task<Result> UpdateAccountAsync(string accountNumber, UpdateBankAccountRequestDto request, CancellationToken ct = default);
        Task<Result> DeleteAccountAsync(string accountNumber, CancellationToken ct = default);
        Task<AccountDetailsResponseDto?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
        Task<AccountDetailsResponseDto?> GetByAccountNumberAndUserIdAsync(string accountNumber, string userId, CancellationToken ct = default);
    }
}
