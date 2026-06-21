using EBanking.Application.Interfaces.Repositories;
using EBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EBanking.Infrastructure.Persistence.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly EBankingDbContext _dbContext;

        public CurrencyRepository(EBankingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Currency?> GetRatesAsync(CancellationToken ct = default)
        {
            return await _dbContext.Currencies.OrderByDescending(c => c.RetrievedAtUtc).SingleOrDefaultAsync(ct);
        }
    }
}
