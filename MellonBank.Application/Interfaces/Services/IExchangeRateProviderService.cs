using MellonBank.Application.DTOs.Responses;

namespace MellonBank.Application.Interfaces.Services
{
    public interface IExchangeRateProviderService
    {
        Task<ConversionResponseDto?> GetConvertedRatesAsync(string fromCurrency, string toCurrency, decimal amount, CancellationToken ct = default);
    }
}
