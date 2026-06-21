using FluentValidation;
using EBanking.Application.Common.Models;
using EBanking.Application.DTOs.Requests;
using EBanking.Application.DTOs.Responses;
using EBanking.Application.Exceptions;
using EBanking.Application.Interfaces.Persistence;
using EBanking.Application.Interfaces.Repositories;
using EBanking.Application.Interfaces.Services;
using EBanking.Domain.Entities;
using EBanking.Domain.Enums;

namespace EBanking.Application.Services
{
    public sealed class AccountManagementService : IAccountManagementService
    {
        private readonly IValidator<CreateBankAccountRequestDto> _createValidator;
        private readonly IValidator<UpdateBankAccountRequestDto> _updateValidator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;
        private readonly IRoleService _roleManagerService;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IAccountNumberGenerator _accountNumberGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public AccountManagementService(
            IValidator<CreateBankAccountRequestDto> createValidator,
            IValidator<UpdateBankAccountRequestDto> updateValidator,
            ICurrentUserService currentUserService,
            IIdentityService identityService,
            IRoleService roleManagerService,
            IBankAccountRepository bankAccountRepository,
            IAccountNumberGenerator accountNumberGenerator,
            IUnitOfWork unitOfWork)
        {
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _currentUserService = currentUserService;
            _identityService = identityService;
            _roleManagerService = roleManagerService;
            _bankAccountRepository = bankAccountRepository;
            _accountNumberGenerator = accountNumberGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AccountDetailsResponseDto>> GetAllAsync(CancellationToken ct = default)
        {
            EnsureAuthenticatedStaff();

            return await _bankAccountRepository.GetAllAsync(ct);
        }

        public async Task<AccountDetailsResponseDto?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default)
        {
            EnsureAuthenticatedStaff();
            ValidateAccountNumber(accountNumber);

            var account = await _bankAccountRepository.GetDetailsByAccountNumberAsync(accountNumber, ct);
            if (account is null)
                throw new AppNotFoundException("Account with given number has not found.");

            return account;
        }

        public async Task<AccountDetailsResponseDto?> GetByAccountNumberAndUserIdAsync(string accountNumber, string userId, CancellationToken ct = default)
        {
            EnsureAuthenticatedStaff();
            ValidateAccountNumber(accountNumber);

            if (string.IsNullOrWhiteSpace(userId))
                throw new AppValidationException("User ID must be provided.");

            var account = await _bankAccountRepository.GetByAccountNumberAndUserIdAsync(accountNumber, userId, ct);
            if (account is null)
                throw new AppNotFoundException("Account with given number has not found for the specified user.");

            return new AccountDetailsResponseDto(
                Id: account.Id,
                AccountNumber: account.AccountNumber,
                Balance: account.Balance,
                Currency: account.Currency,
                Branch: account.Branch,
                AccountType: account.AccountType
            );
        }

        public async Task<Result<Guid>> CreateAccountAsync(CreateBankAccountRequestDto request, CancellationToken ct = default)
        {
            EnsureAuthenticatedStaff();

            var validationResult = await _createValidator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
            {
                return Result<Guid>.Failure(validationResult.Errors.First().ErrorMessage);
            }

            var customer = await _identityService.GetByAfmAsync(request.CustomerAfm, ct);
            if (customer is null)
                return Result<Guid>.Failure("Customer not found.");

            var isCustomer = await _roleManagerService.IsInRoleAsync(customer.Id, RoleType.Customer, ct);
            if (!isCustomer)
                return Result<Guid>.Failure("Bank accounts can only be assigned to customers.");

            string accountNumber;
            do
            {
                accountNumber = _accountNumberGenerator.Generate();
            }
            while (await _bankAccountRepository.ExistsByAccountNumberAsync(accountNumber, ct));

            var account = new BankAccount(
                accountNumber,
                request.InitialBalance,
                request.Currency,
                customer.Id,
                request.Branch,
                request.AccountType);

            await _bankAccountRepository.AddAsync(account, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<Guid>.Success(account.Id);
        }

        public async Task<Result> UpdateAccountAsync(string accountNumber, UpdateBankAccountRequestDto request, CancellationToken ct = default)
        {
            EnsureAuthenticatedStaff();
            ValidateAccountNumber(accountNumber);

            var validationResult = await _updateValidator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
            {
                return Result.Failure(validationResult.Errors.First().ErrorMessage);
            }

            var account = await GetRequiredAccountEntityByAccountNumberAsync(accountNumber, ct);
            if (account is null)
                return Result.Failure("Account with given number has not found.");

            account.UpdateDetails(request.Branch!, request.AccountType!.Value);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<Result> DeleteAccountAsync(string accountNumber, CancellationToken ct = default)
        {
            EnsureAuthenticatedStaff();
            ValidateAccountNumber(accountNumber);

            var account = await GetRequiredAccountEntityByAccountNumberAsync(accountNumber, ct);
            if (account is null)
                return Result.Failure("Account with given number has not found.");

            account.Deactivate();
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        private void EnsureAuthenticatedStaff()
        {
            if (_currentUserService.UserId is null)
                throw new AppForbiddenException("User must be authenticated to access account details.");

            if (!_currentUserService.IsInRole(RoleType.Staff.ToString()))
                throw new AppForbiddenException("Only staff users can manage bank accounts.");
        }

        private static void ValidateAccountNumber(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new AppValidationException("Account number must be provided.");
        }

        private async Task<BankAccount?> GetRequiredAccountEntityByAccountNumberAsync(string accountNumber, CancellationToken ct)
        {
            return await _bankAccountRepository.GetByAccountNumberAsync(accountNumber, ct);
        }
    }
}