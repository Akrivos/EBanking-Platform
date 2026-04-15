using MellonBank.Application.DTOs.Responses;
using MellonBank.Domain.Entities;

namespace MellonBank.Application.Interfaces.Repositories
{
    public interface ICurrencyRepository
    {
        Task<Currency?> GetRatesAsync(CancellationToken ct = default);
    }
}
