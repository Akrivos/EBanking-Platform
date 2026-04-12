using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.DTOs.Responses;
using MellonBank.Application.Models.MellonBank.Application.Models;
using MellonBank.Domain.Enums;

namespace MellonBank.Application.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<string> CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default);
        Task<bool> UpdateUserAsync(string afm, UpdateUserRequestDto request, CancellationToken cancellationToken = default);
        Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
        Task<UserIdentityModel> GetByAfmAsync(string afm, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserIdentityModel?>> GetAllCustomersAsync(CancellationToken cancellationToken = default);
        Task<bool> IsInRoleAsync(string userId, RoleType roleName, CancellationToken cancellationToken = default);
        Task<bool> AddToRoleAsync(string userId, RoleType roleName, CancellationToken cancellationToken = default);
    }
}
