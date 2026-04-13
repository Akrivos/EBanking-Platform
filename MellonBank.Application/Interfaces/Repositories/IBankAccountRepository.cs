using MellonBank.Domain.Entities;

namespace MellonBank.Application.Interfaces.Repositories
{
    public interface IBankAccountRepository
    {
        Task<BankAccount?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
        Task<BankAccount?> GetByAccountNumberAndUserIdAsync(string accountNumber, string userId, CancellationToken ct = default);
        Task<IEnumerable<BankAccount>> GetByUserIdAsync(string userId, CancellationToken ct = default);
        Task<bool> ExistsByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
        Task AddAsync(BankAccount account, CancellationToken ct = default);
        Task UpdateAsync(BankAccount account, CancellationToken ct = default);
        Task DeleteByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
    }
}
