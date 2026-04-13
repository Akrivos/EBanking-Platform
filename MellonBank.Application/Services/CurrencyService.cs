using MellonBank.Application.DTOs.Responses;
using MellonBank.Application.Exceptions;
using MellonBank.Application.Interfaces.Repositories;
using MellonBank.Application.Interfaces.Services;

namespace MellonBank.Application.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<CurrencyResponseDto> GetLatestRatesAsync(CancellationToken ct)
        {
            var rates = await _currencyRepository.GetLatestRatesAsync(ct);
            if(rates is null)
                throw new AppNotFoundException("Currency rates not found.");

            return new CurrencyResponseDto(rates.AUD, rates.CHF, rates.GBP, rates.USD);
        }
    }
}
