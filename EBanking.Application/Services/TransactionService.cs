using EBanking.Application.Common.Models;
using EBanking.Application.DTOs.Responses;
using EBanking.Application.Exceptions;
using EBanking.Application.Interfaces.Repositories;
using EBanking.Application.Interfaces.Services;
using EBanking.Domain.Entities;
using EBanking.Domain.Enums;

namespace EBanking.Application.Services;

public sealed class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ICurrentUserService _currentUserService;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IBankAccountRepository bankAccountRepository,
        ICurrentUserService currentUserService)
    {
        _transactionRepository = transactionRepository;
        _bankAccountRepository = bankAccountRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PagedResult<TransactionResponseDto>> GetAccountTransactionsAsync(
        string accountNumber,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        EnsureAuthenticated();

        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new AppValidationException("Account number must be provided.");

        if (page <= 0)
            throw new AppValidationException("Page must be greater than zero.");

        if (pageSize <= 0)
            throw new AppValidationException("Page size must be greater than zero.");

        await EnsureCanAccessAccountAsync(accountNumber, ct);

        var result = await _transactionRepository
            .GetByAccountNumberAsync(accountNumber, page, pageSize, ct);

        return new PagedResult<TransactionResponseDto>(
            result.Items,
            result.TotalCount,
            page,
            pageSize
        );
    }

    public async Task<TransactionResponseDto> GetTransactionDetailsAsync(
        Guid transactionId,
        CancellationToken ct = default)
    {
        EnsureAuthenticated();

        if (transactionId == Guid.Empty)
            throw new AppValidationException("Transaction id must be provided.");

        var transaction = await _transactionRepository.GetByIdWithAccountsAsync(transactionId, ct);

        if (transaction is null)
            throw new AppNotFoundException("Transaction not found.");

        await EnsureCanAccessTransactionAsync(transaction, ct);

        return MapToDto(transaction);
    }

    private async Task EnsureCanAccessAccountAsync(
        string accountNumber,
        CancellationToken ct)
    {
        if (_currentUserService.IsInRole(RoleType.Staff.ToString()))
            return;

        if (_currentUserService.IsInRole(RoleType.Customer.ToString()))
        {
            var belongsToUser = await _bankAccountRepository.GetByAccountNumberAndUserIdAsync(accountNumber, _currentUserService.UserId!, ct);

            if (belongsToUser is null)
                throw new AppForbiddenException("You are not allowed to view this account's transactions.");

            return;
        }

        throw new AppForbiddenException("You are not allowed to view transactions.");
    }

    private async Task EnsureCanAccessTransactionAsync(
        Transaction transaction,
        CancellationToken ct)
    {
        if (_currentUserService.IsInRole(RoleType.Staff.ToString()))
            return;

        if (_currentUserService.IsInRole(RoleType.Customer.ToString()))
        {
            var userId = _currentUserService.UserId!;

            var canAccessFromAccount = await _bankAccountRepository.GetByAccountNumberAndUserIdAsync(transaction.FromAccount.AccountNumber, userId, ct);

            var canAccessToAccount = await _bankAccountRepository.GetByAccountNumberAndUserIdAsync(transaction.ToAccount.AccountNumber, userId, ct);

            if (canAccessFromAccount is null && canAccessToAccount is null)
                throw new AppForbiddenException("You are not allowed to view this transaction.");

            return;
        }

        throw new AppForbiddenException("You are not allowed to view transactions.");
    }

    private void EnsureAuthenticated()
    {
        if (_currentUserService.UserId is null)
            throw new AppForbiddenException("User must be authenticated to access transactions.");
    }

    private static TransactionResponseDto MapToDto(Transaction transaction)
    {
        return new TransactionResponseDto(
            Id: transaction.Id,
            ReferenceCode: transaction.ReferenceCode,
            FromAccountNumber: transaction.FromAccount.AccountNumber,
            ToAccountNumber: transaction.ToAccount.AccountNumber,
            Type: transaction.Type,
            Status: transaction.Status,
            Amount: transaction.Amount,
            Description: transaction.Description,
            CreatedAt: transaction.CreatedAt
        );
    }
}