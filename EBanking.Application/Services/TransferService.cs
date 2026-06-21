using FluentValidation;
using EBanking.Application.Common.Models;
using EBanking.Application.DTOs.Requests;
using EBanking.Application.Exceptions;
using EBanking.Application.Interfaces.Persistence;
using EBanking.Application.Interfaces.Repositories;
using EBanking.Application.Interfaces.Services;
using EBanking.Domain.Entities;

namespace EBanking.Application.Services
{
    public sealed class TransferService : ITransferService
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
            IUnitOfWork unitOfWork)
        {
            _ownAccountValidator = ownAccountValidator;
            _thirdPartyValidator = thirdPartyValidator;
            _currentUserService = currentUserService;
            _bankAccountRepository = bankAccountRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> TransferToOwnAccountAsync(
            TransferToOwnAccountRequestDto request,
            CancellationToken ct = default)
        {
            var validationResult = await _ownAccountValidator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
                return Result.Failure("Please provide valid transfer details.");

            var userId = GetCurrentUserIdOrThrow();

            var fromAccountNumber = request.FromAccountNumber?.Trim();
            var toAccountNumber = request.ToAccountNumber?.Trim();

            var fromAccount = await _bankAccountRepository
                .GetByAccountNumberAndUserIdAsync(fromAccountNumber!, userId, ct);

            if (fromAccount is null)
                return Result.Failure("Source account not found.");

            var toAccount = await _bankAccountRepository
                .GetByAccountNumberAndUserIdAsync(toAccountNumber!, userId, ct);

            if (toAccount is null)
                return Result.Failure("Destination account not found.");

            if (fromAccount.Id == toAccount.Id)
                return Result.Failure("Source and destination accounts cannot be the same.");

            try
            {
                fromAccount.Debit(request.Amount);
                toAccount.Credit(request.Amount);
            }
            catch (Exception)
            {
                return Result.Failure("Insufficient available balance.");
            }

            var transaction = Transaction.CreateOwnTransfer(
                fromAccount.Id,
                toAccount.Id,
                request.Amount,
                $"Transfer from {fromAccount.AccountNumber} to {toAccount.AccountNumber}",
                userId);

            transaction.MarkCompleted();

            await _transactionRepository.AddAsync(transaction, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<Result> TransferToThirdPartyAsync(
            TransferToThirdPartyRequestDto request,
            CancellationToken ct = default)
        {
            var validationResult = await _thirdPartyValidator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
                return Result.Failure("Please provide valid transfer details.");

            var userId = GetCurrentUserIdOrThrow();

            var fromAccountNumber = request.FromAccountNumber?.Trim();
            var toAccountNumber = request.ToAccountNumber?.Trim();

            var fromAccount = await _bankAccountRepository
                .GetByAccountNumberAndUserIdAsync(fromAccountNumber!, userId, ct);

            if (fromAccount is null)
                return Result.Failure("Source account not found.");

            var toAccount = await _bankAccountRepository
                .GetByAccountNumberAsync(toAccountNumber!, ct);

            if (toAccount is null)
                return Result.Failure("Destination account not found.");

            if (toAccount.UserId == userId)
                return Result.Failure(
                    "Destination account belongs to you. Use own account transfer instead.");

            try
            {
                fromAccount.Debit(request.Amount);
                toAccount.Credit(request.Amount);
            }
            catch (Exception)
            {
                return Result.Failure("Insufficient available balance.");
            }

            var transaction = Transaction.CreateThirdPartyTransfer(
                fromAccount.Id,
                toAccount.Id,
                request.Amount,
                $"Third-party transfer from {fromAccount.AccountNumber} to {toAccount.AccountNumber}",
                userId);

            transaction.MarkCompleted();

            await _transactionRepository.AddAsync(transaction, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        private string GetCurrentUserIdOrThrow()
        {
            if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
                throw new AppForbiddenException("User is not authenticated.");

            return _currentUserService.UserId;
        }
    }
}