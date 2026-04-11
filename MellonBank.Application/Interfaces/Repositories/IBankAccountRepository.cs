using MellonBank.Application.DTOs.Responses;
using MellonBank.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MellonBank.Application.Interfaces.Repositories
{
    public interface IBankAccountRepository
    {
        Task<bool> ExistsByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
        Task AddAsync(BankAccount account, CancellationToken ct = default);
        Task<BankAccount?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
        Task DeleteByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
    }
}
