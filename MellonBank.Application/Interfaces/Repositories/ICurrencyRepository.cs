using MellonBank.Domain.Entities;

namespace MellonBank.Application.Interfaces.Repositories
{
    public interface ICurrencyRepository
    {
        Task<Currency?> GetLatestRatesAsync(CancellationToken ct = default);
    }
}
