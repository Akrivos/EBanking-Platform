using MellonBank.Application.Interfaces.Persistence;

namespace MellonBank.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MellonBankDbContext _context;

        public UnitOfWork(MellonBankDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
