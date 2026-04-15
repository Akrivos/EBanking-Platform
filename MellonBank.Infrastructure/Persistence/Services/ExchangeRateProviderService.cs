using MellonBank.Application.DTOs.Responses;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace MellonBank.Infrastructure.Persistence.Services
{
    public sealed class ExchangeRateProviderService : IExchangeRateProviderService
    {
        private readonly HttpClient _httpClient;
        private readonly ExchangeRateApiOptions _options;

        public ExchangeRateProviderService(
            HttpClient httpClient,
            IOptions<ExchangeRateApiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<ConversionResponseDto?> GetConvertedRatesAsync(
            string fromCurrency,
            string toCurrency,
            decimal amount,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(fromCurrency))
                throw new ArgumentException("From currency is required.", nameof(fromCurrency));

            if (string.IsNullOrWhiteSpace(toCurrency))
                throw new ArgumentException("To currency is required.", nameof(toCurrency));

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            if (string.IsNullOrWhiteSpace(_options.ApiKey))
                throw new InvalidOperationException("ExchangeRate API key is not configured.");

            var url = $"{_options.BaseUrl}{_options.ApiKey}/pair/{fromCurrency}/{toCurrency}/{amount}";

            using var response = await _httpClient.GetAsync(url, ct);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(ct);
                throw new InvalidOperationException("Failed to convert currency");
            }

            var data = await response.Content.ReadFromJsonAsync<ConversionResponseDto>(cancellationToken: ct);

            if (data is null)
                throw new InvalidOperationException("Exchange rate API returned an empty response.");

            return data;
        }
    }
}