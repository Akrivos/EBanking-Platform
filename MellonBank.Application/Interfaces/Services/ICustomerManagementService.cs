using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.DTOs.Responses;

namespace MellonBank.Application.Interfaces.Services
{
    public interface ICustomerManagementService
    {
        Task<Guid> CreateCustomerAsync(CreateUserRequestDto request, CancellationToken ct = default);
        Task<Guid> CreateStaffAsync(CreateUserRequestDto request, CancellationToken ct = default);
        Task UpdateCustomerAsync(UpdateCustomerRequestDto request, CancellationToken ct = default);
        Task DeleteCustomerAsync(string afm, CancellationToken ct = default);
        Task<CustomerDetailsResponseDto?> GetCustomerByAfmAsync(string afm, CancellationToken ct = default);
        Task<IReadOnlyList<CustomerDetailsResponseDto?>> GetAllCustomersAsync(CancellationToken ct = default);
    }
}
