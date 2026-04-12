using MellonBank.Domain.Entities;

namespace MellonBank.Application.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction, CancellationToken ct = default);
    }
}
