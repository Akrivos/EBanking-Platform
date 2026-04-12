using MellonBank.Application.DTOs.Responses;
using MellonBank.Application.Exceptions;
using MellonBank.Application.Interfaces.Repositories;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Domain.Enums;

namespace MellonBank.Application.Services
{
    public class CustomerAccountService : ICustomerAccountService
    {
        private readonly ICurrencyService _currencyService;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICurrentUserService _currentUserService;
        public CustomerAccountService(
            ICurrencyService currencyService, 
            IBankAccountRepository bankAccountRepository,
            ICurrentUserService currentUserService
            )
        {
            _currencyService = currencyService;
            _bankAccountRepository = bankAccountRepository;
            _currencyService = currencyService;
        }

        public async Task<BalanceInCurrenciesResponseDto?> GetBalanceInCurrenciesAsync(string accountNumber, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new AppValidationException("Account number is required.");

            if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
                throw new AppForbiddenException("User is not authenticated.");

            var bankAccount = await _bankAccountRepository.GetByAccountNumberAndUserIdAsync(accountNumber, _currentUserService.UserId, ct);

            if (bankAccount is null)
                throw new AppNotFoundException("Account has not found or does not belong to the specified user.");

            var euroToUsdRate = await _currencyService.GetExchangeRateAsync(CurrencyType.EUR, CurrencyType.USD, ct);

            if (euroToUsdRate <= 0)
                throw new ExternalServiceException("Failed to retrieve exchange rate from the currency service.");

            var balanceUsd = Math.Round(bankAccount.Balance * euroToUsdRate,2, MidpointRounding.AwayFromZero);

            return new BalanceInCurrenciesResponseDto(
                AccountNumber: bankAccount.AccountNumber,
                BalanceEuro: bankAccount.Balance,
                EuroToUsdRate: euroToUsdRate,
                BalanceUsd: balanceUsd);
        }

    }
}
