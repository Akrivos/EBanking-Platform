using EBanking.Application.Common.Models;
using EBanking.Application.DTOs.Responses;
using EBanking.Domain.Entities;

namespace EBanking.Application.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction, CancellationToken ct = default);
        Task<PagedData<TransactionResponseDto>> GetByAccountNumberAsync(
            string accountNumber,
            int page,
            int pageSize,
            CancellationToken ct = default
        );

        Task<Transaction?> GetByIdWithAccountsAsync(
            Guid transactionId,
            CancellationToken ct = default
        );
    }
}
