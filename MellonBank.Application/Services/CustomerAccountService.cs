using MellonBank.Application.DTOs.Responses;
using MellonBank.Application.Exceptions;
using MellonBank.Application.Interfaces.Repositories;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Domain.Enums;

namespace MellonBank.Application.Services
{
    public class CustomerAccountService : ICustomerAccountService
    {
        private readonly IExchangeRateProviderService _exchangeRateProvider;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICurrentUserService _currentUserService;
        public CustomerAccountService(
            IExchangeRateProviderService exchangeRateProvider,
            IBankAccountRepository bankAccountRepository,
            ICurrentUserService currentUserService
            )
        {
            _exchangeRateProvider = exchangeRateProvider;
            _bankAccountRepository = bankAccountRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ConversionResponseDto?> GetConvertedBalanceAsync(string accountNumber, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new AppValidationException("Account number is required.");

            if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
                throw new AppForbiddenException("User is not authenticated.");

            var bankAccount = await _bankAccountRepository.GetByAccountNumberAndUserIdAsync(accountNumber, _currentUserService.UserId, ct);

            if (bankAccount is null)
                throw new AppNotFoundException("Account has not found or does not belong to the specified user.");

            var convertedResult = await _exchangeRateProvider.GetConvertedRatesAsync(CurrencyType.EUR.ToString(), CurrencyType.USD.ToString(), bankAccount.Balance, ct);

            if(convertedResult is null)
                throw new AppNotFoundException("Conversion rates not found for the specified currencies.");

            return convertedResult;
        }

        public async Task<IEnumerable<CustomerAccountDetailsResponseDto>> GetAccountsAsync(CancellationToken ct)
        {
            var currentUserId = _currentUserService.UserId;
            if (string.IsNullOrWhiteSpace(currentUserId))
                throw new AppForbiddenException("User is not authenticated.");

            var bankAccounts = await _bankAccountRepository.GetByUserIdAsync(currentUserId, ct);

            if(bankAccounts is null)
                throw new AppNotFoundException("No accounts found for the specified user.");

            return bankAccounts.Select(acc => new CustomerAccountDetailsResponseDto(
                AccountNumber: acc.AccountNumber,
                Balance: acc.Balance,
                Currency: acc.Currency,
                Branch: acc.Branch,
                AccountType: acc.AccountType
             )).ToList();

        }

        public async Task<CustomerAccountDetailsResponseDto?> GetAccountDetailsAsync(string accountNumber, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new AppValidationException("Account number is required.");

            if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
                throw new AppForbiddenException("User is not authenticated.");

            var bankAccount = await _bankAccountRepository.GetByAccountNumberAndUserIdAsync(accountNumber, _currentUserService.UserId, ct);
            if (bankAccount is null)
                throw new AppNotFoundException("Account has not found or does not belong to the specified user.");

            return new CustomerAccountDetailsResponseDto(
                AccountNumber: bankAccount.AccountNumber,
                Balance: bankAccount.Balance,
                Currency: bankAccount.Currency,
                Branch: bankAccount.Branch,
                AccountType: bankAccount.AccountType
            );
        }

    }
}
