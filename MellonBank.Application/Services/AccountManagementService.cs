using FluentValidation;
using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.DTOs.Responses;
using MellonBank.Application.Exceptions;
using MellonBank.Application.Interfaces.Persistence;
using MellonBank.Application.Interfaces.Repositories;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Domain.Entities;
using MellonBank.Domain.Enums;

namespace MellonBank.Application.Services
{
    public class AccountManagementService : IAccountManagementService
    {
        private readonly IValidator<CreateBankAccountRequestDto> _createValidator;
        private readonly IValidator<UpdateBankAccountRequestDto> _updateValidator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AccountManagementService(
            IValidator<CreateBankAccountRequestDto> createValidator,
            IValidator<UpdateBankAccountRequestDto> updateValidator,
            ICurrentUserService currentUserService,
            IIdentityService identityService,
            IBankAccountRepository bankAccountRepository,
            IUnitOfWork unitOfWork
        )
        {
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _currentUserService = currentUserService;
            _identityService = identityService;
            _bankAccountRepository = bankAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountDetailsResponseDto?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new AppValidationException("Account number must be provided.");

            var account = await _bankAccountRepository.GetByAccountNumberAsync(accountNumber, ct);
            if (account is null)
                throw new AppNotFoundException("Account with given number has not found.");

            return new AccountDetailsResponseDto(
                Id: account.Id,
                AccountNumber: account.AccountNumber,
                Balance: account.Balance,
                Currency: account.Currency,
                Branch: account.Branch,
                AccountType: account.AccountType
            );
        }

        public async Task<Guid> CreateAccountAsync(CreateBankAccountRequestDto request, CancellationToken ct = default)
        {
            if (!_currentUserService.IsInRole(RoleType.Staff.ToString()))
                throw new AppForbiddenException("Only staff users can create bank accounts.");

            var result = await _createValidator.ValidateAsync(request, ct);

            if (!result.IsValid)
                throw new AppValidationException(result.ToDictionary());

            var customer = await _identityService.GetByAfmAsync(request.CustomerAfm, ct);
            if (customer is null)
                throw new AppNotFoundException("Customer not found.");

            var isCustomer = await _identityService.IsInRoleAsync(customer.Id, RoleType.Customer, ct);
            if (!isCustomer)
                throw new AppConflictException("Bank accounts can only be assigned to customers.");

            var accountExists = await _bankAccountRepository.ExistsByAccountNumberAsync(request.AccountNumber, ct);
            if (accountExists)
                throw new AppConflictException("An account with the same account number already exists.");

            var account = new BankAccount(
                request.AccountNumber,
                request.InitialBalance,
                request.Currency,
                customer.Id,
                request.Branch,
                request.AccountType);

            await _bankAccountRepository.AddAsync(account, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return account.Id;
        }

        public async Task UpdateAccountAsync(string accountNumber, UpdateBankAccountRequestDto request, CancellationToken ct = default)
        {
            if (!_currentUserService.IsInRole(RoleType.Staff.ToString()))
                throw new AppForbiddenException("Only staff users can update bank accounts.");

            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new AppValidationException("Account number must be provided.");

            var result = await _updateValidator.ValidateAsync(request, ct);
            if (!result.IsValid)
                throw new AppValidationException(result.ToDictionary());

            var account = await _bankAccountRepository.GetByAccountNumberAsync(accountNumber, ct);
            if (account is null)
                throw new AppNotFoundException("Account with given number has not found.");

            account.UpdateDetails(request.Branch!, request.AccountType!.Value);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAccountAsync(string accountNumber, CancellationToken ct = default)
        {
            if (!_currentUserService.IsInRole(RoleType.Staff.ToString()))
                throw new AppForbiddenException("Only staff users can delete bank accounts.");

            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new AppValidationException("Account number must be provided.");

            var account = await _bankAccountRepository.GetByAccountNumberAsync(accountNumber, ct);
            if (account is null)
                throw new AppNotFoundException("Account with given number has not found.");

            await _bankAccountRepository.DeleteByAccountNumberAsync(accountNumber, ct);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
