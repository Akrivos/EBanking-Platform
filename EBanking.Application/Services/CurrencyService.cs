using EBanking.Application.DTOs.Responses;
using EBanking.Application.Exceptions;
using EBanking.Application.Interfaces.Repositories;
using EBanking.Application.Interfaces.Services;

namespace EBanking.Application.Services
{
    public sealed class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<CurrencyResponseDto?> GetRatesAsync(CancellationToken ct)
        {
            var result = await _currencyRepository.GetRatesAsync(ct);
            if (result is null) 
                throw new AppNotFoundException("Currency rates not found.");

            return new CurrencyResponseDto(
                result.AUD,
                result.CHF,
                result.GBP,
                result.USD
            );
        }
    }
}
