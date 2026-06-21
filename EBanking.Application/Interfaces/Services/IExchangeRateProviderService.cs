using EBanking.Application.DTOs.Responses;

namespace EBanking.Application.Interfaces.Services
{
    public interface IExchangeRateProviderService
    {
        Task<ConversionResponseDto?> GetConvertedRatesAsync(string fromCurrency, string toCurrency, decimal amount, CancellationToken ct = default);
    }
}
