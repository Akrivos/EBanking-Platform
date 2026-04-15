using MellonBank.Application.Interfaces.Repositories;
using MellonBank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MellonBank.Infrastructure.Persistence.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly MellonBankDbContext _dbContext;

        public CurrencyRepository(MellonBankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Currency?> GetRatesAsync(CancellationToken ct = default)
        {
            return await _dbContext.Currencies.OrderByDescending(c => c.RetrievedAtUtc).SingleOrDefaultAsync(ct);
        }
    }
}
