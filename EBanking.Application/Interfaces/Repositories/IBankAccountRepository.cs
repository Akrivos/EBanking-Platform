using EBanking.Application.DTOs.Responses;
using EBanking.Domain.Entities;

namespace EBanking.Application.Interfaces.Repositories
{
    public interface IBankAccountRepository
    {
        Task<IEnumerable<AccountDetailsResponseDto>> GetAllAsync(CancellationToken ct = default);
        Task<BankAccount?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
        Task<AccountDetailsResponseDto?> GetDetailsByAccountNumberAsync(string accountNumber, CancellationToken ct);
        Task<BankAccount?> GetByAccountNumberAndUserIdAsync(string accountNumber, string userId, CancellationToken ct = default);
        Task<IEnumerable<BankAccount>> GetByUserIdAsync(string userId, CancellationToken ct = default);
        Task<bool> ExistsByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
        Task<bool> AnyAccountByUserIdAsync(string userId, CancellationToken ct = default);
        Task AddAsync(BankAccount account, CancellationToken ct = default);
        Task UpdateAsync(BankAccount account, CancellationToken ct = default);
        Task DeleteByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
    }
}
