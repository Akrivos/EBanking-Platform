using MellonBank.Domain.Enums;

namespace MellonBank.Application.Interfaces.Services
{
    public interface IExchangeRateProvider
    {
        Task<decimal> GetExchangeRateAsync(CurrencyType from, CurrencyType to, CancellationToken ct = default);
    }
}
