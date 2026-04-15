using MellonBank.Application.DTOs.Responses;

namespace MellonBank.Application.Interfaces.Services
{
    public interface ICurrencyService
    {
        Task<CurrencyResponseDto?> GetRatesAsync(CancellationToken ct = default);
    }
}
