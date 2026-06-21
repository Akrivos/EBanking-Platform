using EBanking.Application.Common.Models;
using EBanking.Application.DTOs.Requests;
using EBanking.Application.DTOs.Responses;

namespace EBanking.Application.Interfaces.Services
{
    public interface IStaffUserManagementService
    {
        Task<Result<string>> CreateCustomerAsync(CreateUserRequestDto request, CancellationToken ct = default);
        Task<Result<string>> CreateStaffAsync(CreateUserRequestDto request, CancellationToken ct = default);
        Task<Result> UpdateCustomerAsync(string afm, UpdateUserRequestDto request, CancellationToken ct = default);
        Task<Result> DeleteCustomerAsync(string afm, CancellationToken ct = default);
        Task<UserResponseDto?> GetCustomerByAfmAsync(string afm, CancellationToken ct = default);
        Task<IEnumerable<UserResponseDto?>> GetAllCustomersAsync(CancellationToken ct = default);
    }
}
