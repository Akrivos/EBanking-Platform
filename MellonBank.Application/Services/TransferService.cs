using FluentValidation;
using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.Exceptions;
using MellonBank.Application.Interfaces.Persistence;
using MellonBank.Application.Interfaces.Repositories;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Domain.Entities;

namespace MellonBank.Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly IValidator<TransferToOwnAccountRequestDto> _ownAccountValidator;
        private readonly IValidator<TransferToThirdPartyRequestDto> _thirdPartyValidator;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferService(
            IValidator<TransferToOwnAccountRequestDto> ownAccountValidator,
            IValidator<TransferToThirdPartyRequestDto> thirdPartyValidator,
            ICurrentUserService currentUserService, 
            IBankAccountRepository bankAccountRepository,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork
        )
        {
            _ownAccountValidator = ownAccountValidator;
            _thirdPartyValidator = thirdPartyValidator;
            _bankAccountRepository = bankAccountRepository;
            _currentUserService = currentUserService;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task TransferToOwnAccountAsync(
           TransferToOwnAccountRequestDto request,
            CancellationToken ct = default)
        {
            var fromAccountNumber = request.FromAccountNumber?.Trim();
            var toAccountNumber = request.ToAccountNumber?.Trim();
            var amount = request.Amount;

            var validationResult = await _ownAccountValidator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
                throw new AppValidationException(validationResult.ToDictionary());

            var userId = GetCurrentUserIdOrThrow();

            var fromAccount = await GetSourceAccountAsync(fromAccountNumber, userId, ct);

            var toAccount = await _bankAccountRepository.GetByAccountNumberAndUserIdAsync(toAccountNumber, userId, ct);

            if (toAccount is null)
                throw new AppNotFoundException("Destination account not found.");

            fromAccount.Debit(amount);
            toAccount.Credit(amount);

            var transaction = Transaction.CreateOwnTransfer(
                fromAccount.Id,
                toAccount.Id,
                amount,
                $"Transfer from {fromAccount.AccountNumber} to {toAccount.AccountNumber}",
                userId);

            transaction.MarkCompleted();

            await _transactionRepository.AddAsync(transaction, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task TransferToThirdPartyAsync(
            TransferToThirdPartyRequestDto request,
            CancellationToken ct = default)
        {
            var fromAccountNumber = request.FromAccountNumber?.Trim();
            var toAccountNumber = request.ToAccountNumber?.Trim();
            var amount = request.Amount;

            var validationResult = await _thirdPartyValidator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
                throw new AppValidationException(validationResult.ToDictionary());

            var userId = GetCurrentUserIdOrThrow();

            var fromAccount = await GetSourceAccountAsync(fromAccountNumber!, userId, ct);

            var toAccount = await GetDestinationAccountAsync(toAccountNumber!, ct);

            if (toAccount.UserId == userId)
                throw new AppValidationException(
                    "Destination account belongs to the current user. Use own account transfer instead.");

            fromAccount.Debit(amount);
            toAccount.Credit(amount);

            var transaction = Transaction.CreateThirdPartyTransfer(
                fromAccount.Id,
                toAccount.Id,
                amount,
                $"Third-party transfer from {fromAccount.AccountNumber} to {toAccount.AccountNumber}",
                userId);

            transaction.MarkCompleted();

            await _transactionRepository.AddAsync(transaction, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        private string GetCurrentUserIdOrThrow()
        {
            if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
                throw new AppForbiddenException("User is not authenticated.");

            return _currentUserService.UserId;
        }

        private async Task<BankAccount> GetSourceAccountAsync(
            string accountNumber,
            string userId,
            CancellationToken ct)
        {
            var account = await _bankAccountRepository.GetByAccountNumberAndUserIdAsync(accountNumber, userId, ct);

            if (account is null)
                throw new AppNotFoundException("Source account not found.");

            return account;
        }

        private async Task<BankAccount> GetDestinationAccountAsync(
            string accountNumber,
            CancellationToken ct)
        {
            var account = await _bankAccountRepository.GetByAccountNumberAsync(accountNumber, ct);

            if (account is null)
                throw new AppNotFoundException("Destination account not found.");

            return account;
        }
    }
}
