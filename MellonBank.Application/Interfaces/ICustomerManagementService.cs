using System;
using System.Collections.Generic;
using System.Text;

namespace MellonBank.Application.Interfaces
{
    public interface ICustomerManagementService
    {
        Task<string> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);
        Task UpdateCustomerAsync(UpdateCustomerRequest request, CancellationToken cancellationToken = default);
        Task DeleteCustomerAsync(string afm, CancellationToken cancellationToken = default);
        Task<CustomerDetailsDto?> GetCustomerByAfmAsync(string afm, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CustomerListItemDto>> GetAllCustomersAsync(CancellationToken cancellationToken = default);
    }
}
