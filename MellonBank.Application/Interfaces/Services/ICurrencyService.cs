using MellonBank.Application.DTOs.Responses;
using MellonBank.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MellonBank.Application.Interfaces.Services
{
    public interface ICurrencyService
    {
        Task<ExchangeRateResponseDto?> GetLatestEuroToUsdRateAsync(CancellationToken ct = default);
        Task<decimal> GetExchangeRateAsync(CurrencyType from, CurrencyType to, CancellationToken ct = default);
        //Task<ExchangeRateResponseDto> GetLatestRatesAsync(CancellationToken ct = default);
        //Task<decimal> ConvertFromEuroAsync(decimal amount, string targetCurrency, CancellationToken ct = default);
        //Task SaveRatesAsync(ExchangeRateResponseDto rates, CancellationToken ct = default);
        //Task<ExchangeRateResponseDto?> GetStoredRatesAsync(CancellationToken ct = default);
    }
}
