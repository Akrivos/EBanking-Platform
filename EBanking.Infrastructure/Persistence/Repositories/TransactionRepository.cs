using EBanking.Application.Common.Models;
using EBanking.Application.DTOs.Responses;
using EBanking.Application.Interfaces.Repositories;
using EBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EBanking.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly EBankingDbContext _context;

        public TransactionRepository(EBankingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transaction transaction, CancellationToken ct = default)
        {
            await _context.Transactions.AddAsync(transaction, ct);
        }

        public async Task<PagedData<TransactionResponseDto>> GetByAccountNumberAsync(
            string accountNumber,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Transactions
                .AsNoTracking()
                .Where(t =>
                    t.FromAccount.AccountNumber == accountNumber ||
                    t.ToAccount.AccountNumber == accountNumber);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .ThenByDescending(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TransactionResponseDto(
                    t.Id,
                    t.ReferenceCode,
                    t.FromAccount.AccountNumber,
                    t.ToAccount.AccountNumber,
                    t.Type,
                    t.Status,
                    t.Amount,
                    t.Description,
                    t.CreatedAt
                ))
                .ToListAsync(ct);

            return new PagedData<TransactionResponseDto>(
                items,
                totalCount);
        }

        public async Task<Transaction?> GetByIdWithAccountsAsync(
            Guid transactionId,
            CancellationToken ct = default)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .FirstOrDefaultAsync(t => t.Id == transactionId, ct);
        }
    }
}
