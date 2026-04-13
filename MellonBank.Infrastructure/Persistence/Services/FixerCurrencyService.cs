using MellonBank.Application.Interfaces.Services;
using MellonBank.Domain.Enums;
using MellonBank.Infrastructure.Exceptions;
using MellonBank.Infrastructure.Models;
using MellonBank.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace MellonBank.Infrastructure.Persistence.Services
{
    public sealed class FixerCurrencyService : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly FixerOptions _options;

        public FixerCurrencyService(
            HttpClient httpClient,
            IOptions<FixerOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<decimal> GetExchangeRateAsync(
            CurrencyType from,
            CurrencyType to,
            CancellationToken ct = default)
        {
            if (from == to)
                return 1m;

            var fromCode = from.ToString();
            var toCode = to.ToString();

            var requestUri = $"latest?access_key={_options.ApiKey}&symbols={fromCode},{toCode}";

            FixerLatestResponse? response;
            try
            {
                response = await _httpClient.GetFromJsonAsync<FixerLatestResponse>(requestUri, ct);
            }
            catch (Exception ex)
            {
                throw new ExternalServiceException($"Failed to communicate with the Fixer API. {ex.Message}");
            }

            if (response is null)
                throw new ExternalServiceException("The Fixer API returned an empty response.");

            if (!response.Success)
            {
                var message = response.Error is null
                    ? "The Fixer API returned an unsuccessful response."
                    : $"Fixer API error: {response.Error.Type} - {response.Error.Info}";

                throw new ExternalServiceException(message);
            }

            if (from == CurrencyType.EUR)
            {
                if (!response.Rates.TryGetValue(toCode, out var directRate))
                    throw new ExternalServiceException($"Exchange rate for {toCode} was not found.");

                if (directRate <= 0)
                    throw new ExternalServiceException("The Fixer API returned an invalid exchange rate.");

                return directRate;
            }

            if (!response.Rates.TryGetValue(fromCode, out var fromRate))
                throw new ExternalServiceException($"Exchange rate for {fromCode} was not found.");

            if (!response.Rates.TryGetValue(toCode, out var toRate))
                throw new ExternalServiceException($"Exchange rate for {toCode} was not found.");

            if (fromRate <= 0 || toRate <= 0)
                throw new ExternalServiceException("The Fixer API returned invalid exchange rates.");

            if (to == CurrencyType.EUR)
                return 1m / fromRate;

            return toRate / fromRate;
        }
    }
}