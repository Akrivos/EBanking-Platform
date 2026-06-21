using EBanking.Application.Interfaces.Repositories;
using EBanking.Domain.Entities;

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
    }
}
