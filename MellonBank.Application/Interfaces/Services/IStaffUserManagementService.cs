using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.DTOs.Responses;

namespace MellonBank.Application.Interfaces.Services
{
    public interface IStaffUserManagementService
    {
        Task<string> CreateCustomerAsync(CreateUserRequestDto request, CancellationToken ct = default);
        Task<string> CreateStaffAsync(CreateUserRequestDto request, CancellationToken ct = default);
        Task UpdateCustomerAsync(string afm, UpdateUserRequestDto request, CancellationToken ct = default);
        Task DeleteCustomerAsync(string afm, CancellationToken ct = default);
        Task<UserResponseDto?> GetCustomerByAfmAsync(string afm, CancellationToken ct = default);
        Task<IEnumerable<UserResponseDto?>> GetAllCustomersAsync(CancellationToken ct = default);
    }
}
