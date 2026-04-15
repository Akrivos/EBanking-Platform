using MellonBank.Application.DTOs.Responses;
using MellonBank.Application.Interfaces.Repositories;
using MellonBank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MellonBank.Infrastructure.Persistence.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly MellonBankDbContext _dbContext;

        public BankAccountRepository(MellonBankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<AccountDetailsResponseDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbContext.BankAccounts.Join(
                     _dbContext.Users,
                     ba => ba.UserId,
                     u => u.Id,
                     (ba, u) => new AccountDetailsResponseDto(
                         ba.Id,
                         ba.AccountNumber,
                         ba.Balance,
                         ba.Currency,
                         ba.Branch,
                         ba.AccountType,
                         u.Afm
                     )).ToListAsync(ct);
        }

        public async Task<AccountDetailsResponseDto?> GetDetailsByAccountNumberAsync(string accountNumber, CancellationToken ct = default)
        {
            return await _dbContext.BankAccounts.Where(ba => ba.AccountNumber == accountNumber)
                .Join(
                    _dbContext.Users,
                    ba => ba.UserId,
                    u => u.Id,
                    (ba, u) => new AccountDetailsResponseDto(
                        ba.Id,
                        ba.AccountNumber,
                        ba.Balance,
                        ba.Currency,
                        ba.Branch,
                        ba.AccountType,
                        u.Afm
                    )).SingleOrDefaultAsync(ct);
        }

        public async Task<BankAccount?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default)
        {
            return await _dbContext.BankAccounts.Where(ba => ba.AccountNumber == accountNumber).SingleOrDefaultAsync(ct);
        }

        public async Task<BankAccount?> GetByAccountNumberAndUserIdAsync(string accountNumber, string userId, CancellationToken ct = default)
        {
            return await _dbContext.BankAccounts
                .Where(
                    ba => ba.AccountNumber == accountNumber && 
                    ba.UserId == userId
                )
                .SingleOrDefaultAsync(ct);
        }

        public async Task<IEnumerable<BankAccount>> GetByUserIdAsync(string userId, CancellationToken ct = default)
        {
            return await _dbContext.BankAccounts.Where(ba => ba.UserId == userId).ToListAsync(ct);
        }

        public async Task AddAsync(BankAccount account, CancellationToken ct = default)
        {
            await _dbContext.BankAccounts.AddAsync(account, ct);
        }

        public async Task<bool> ExistsByAccountNumberAsync(string accountNumber, CancellationToken ct = default)
        {
            return await _dbContext.BankAccounts.AnyAsync(ba => ba.AccountNumber == accountNumber, ct);
        }

        public async Task DeleteByAccountNumberAsync(string accountNumber, CancellationToken ct = default)
        {
            var account = await GetByAccountNumberAsync(accountNumber, ct);
            if (account is not null)
            {
                _dbContext.BankAccounts.Remove(account);
            }
        }

        public async Task UpdateAsync(BankAccount account, CancellationToken ct = default)
        {
            _dbContext.BankAccounts.Update(account);
        }
    }
}
