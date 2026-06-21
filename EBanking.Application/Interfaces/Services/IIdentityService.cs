using EBanking.Application.Common.Models;
using EBanking.Application.DTOs.Requests;
using EBanking.Application.Models;
using EBanking.Domain.Enums;

namespace EBanking.Application.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<string?> CreateUserAsync(CreateUserRequestDto request, CancellationToken ct = default);
        Task<bool> UpdateUserAsync(string afm, UpdateUserRequestDto request, CancellationToken ct = default);
        Task<bool> DeleteUserAsync(string userId, CancellationToken ct = default);
        Task<UserIdentityModel?> GetByAfmAsync(string afm, CancellationToken ct = default);
        Task<IEnumerable<UserIdentityModel?>> GetUsersInRoleAsync(RoleType roleName, CancellationToken ct = default);
        Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword, CancellationToken ct = default);
    }
}
