using EBanking.Application.DTOs.Responses;
using EBanking.Application.Interfaces.Services;
using EBanking.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace EBanking.Infrastructure.Persistence.Services
{
    public sealed class ExchangeRateProviderService : IExchangeRateProviderService
    {
        private readonly HttpClient _httpClient;
        private readonly ExchangeRateApiOptions _options;
        private readonly ILogger<ExchangeRateProviderService> _logger;

        public ExchangeRateProviderService(
            HttpClient httpClient,
            IOptions<ExchangeRateApiOptions> options,
            ILogger<ExchangeRateProviderService> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<ConversionResponseDto> GetConvertedRatesAsync(
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
                throw new InvalidOperationException("ExchangeRate api key is not configured.");

            if (string.IsNullOrWhiteSpace(_options.BaseUrl))
                throw new InvalidOperationException("ExchangeRate api baseUrl is not configured.");

            var url = $"{_options.BaseUrl}{_options.ApiKey}/pair/{fromCurrency}/{toCurrency}/{amount}";

            _logger.LogInformation(
                "Requesting currency conversion from {FromCurrency} to {ToCurrency} for amount {Amount}.",
                fromCurrency,
                toCurrency,
                amount);

            try
            {
                using var response = await _httpClient.GetAsync(url, ct);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync(ct);

                    _logger.LogWarning(
                        "Exchange rate API request failed. StatusCode: {StatusCode}. Response: {ResponseBody}",
                        (int)response.StatusCode,
                        errorBody);

                    throw new InvalidOperationException("Failed to convert currency.");
                }

                var data = await response.Content.ReadFromJsonAsync<ConversionResponseDto>(cancellationToken: ct);

                if (data is null)
                {
                    _logger.LogWarning("Exchange rate API returned an empty or invalid response.");
                    throw new InvalidOperationException("Exchange rate API returned an empty response.");
                }

                _logger.LogInformation(
                    "Currency conversion completed successfully. Rate: {Rate}, Result: {Result}",
                    data.ConversionRate,
                    data.ConversionResult);

                return data;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning(
                    "Exchange rate request was canceled for {FromCurrency} to {ToCurrency}.",
                    fromCurrency,
                    toCurrency);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error while converting currency from {FromCurrency} to {ToCurrency} for amount {Amount}.",
                    fromCurrency,
                    toCurrency,
                    amount);

                throw;
            }
        }
    }
}