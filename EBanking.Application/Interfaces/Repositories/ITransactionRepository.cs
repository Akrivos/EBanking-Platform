using EBanking.Domain.Entities;

namespace EBanking.Application.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction, CancellationToken ct = default);
    }
}
