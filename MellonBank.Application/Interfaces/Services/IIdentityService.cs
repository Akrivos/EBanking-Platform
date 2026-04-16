using MellonBank.Application.Common.Models;
using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.Models.MellonBank.Application.Models;

namespace MellonBank.Application.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<string> CreateUserAsync(CreateUserRequestDto request, CancellationToken ct = default);
        Task<bool> UpdateUserAsync(string afm, UpdateUserRequestDto request, CancellationToken ct = default);
        Task<bool> DeleteUserAsync(string userId, CancellationToken ct = default);
        Task<UserIdentityModel> GetByAfmAsync(string afm, CancellationToken ct = default);
        Task<IEnumerable<UserIdentityModel?>> GetUsersInRoleAsync(CancellationToken ct = default);
        Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword, CancellationToken ct = default);
    }
}
