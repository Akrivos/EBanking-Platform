using MellonBank.Application.DTOs.Responses;
using MellonBank.Application.Exceptions;
using MellonBank.Application.Interfaces.Repositories;
using MellonBank.Application.Interfaces.Services;

namespace MellonBank.Application.Services
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
