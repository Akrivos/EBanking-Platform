using EBanking.Application.DTOs.Responses;

namespace EBanking.Application.Interfaces.Services
{
    public interface ICurrencyService
    {
        Task<CurrencyResponseDto?> GetRatesAsync(CancellationToken ct = default);
    }
}
