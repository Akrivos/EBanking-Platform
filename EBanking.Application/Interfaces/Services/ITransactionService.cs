using EBanking.Application.Common.Models;
using EBanking.Application.DTOs.Responses;

namespace EBanking.Application.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<PagedResult<TransactionResponseDto>> GetAccountTransactionsAsync(
            string accountNumber,
            int page,
            int pageSize,
            CancellationToken ct = default);

        Task<TransactionResponseDto> GetTransactionDetailsAsync(
            Guid transactionId,
            CancellationToken ct = default);
    }
}
