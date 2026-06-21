using EBanking.Application.Interfaces.Persistence;

namespace EBanking.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EBankingDbContext _context;

        public UnitOfWork(EBankingDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
