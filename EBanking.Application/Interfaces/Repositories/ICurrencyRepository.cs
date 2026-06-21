using EBanking.Application.DTOs.Responses;
using EBanking.Domain.Entities;

namespace EBanking.Application.Interfaces.Repositories
{
    public interface ICurrencyRepository
    {
        Task<Currency?> GetRatesAsync(CancellationToken ct = default);
    }
}
