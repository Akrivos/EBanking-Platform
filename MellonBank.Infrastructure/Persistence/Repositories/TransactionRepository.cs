using MellonBank.Application.Interfaces.Repositories;
using MellonBank.Domain.Entities;

namespace MellonBank.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly MellonBankDbContext _context;

        public TransactionRepository(MellonBankDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transaction transaction, CancellationToken ct = default)
        {
            await _context.Transactions.AddAsync(transaction, ct);
        }
    }
}
