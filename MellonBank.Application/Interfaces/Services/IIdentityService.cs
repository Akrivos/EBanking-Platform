using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.DTOs.Responses;

namespace MellonBank.Application.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<string> CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(UpdateUserRequestDto request, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
        Task<UserResponseDto?> GetByAfmAsync(string afm, CancellationToken cancellationToken = default);
        //Task<UserDto?> GetByIdAsync(string userId, CancellationToken cancellationToken = default);
        //Task<IReadOnlyList<UserDto>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default);
        Task<bool> IsInRoleAsync(string userId, string roleName, CancellationToken cancellationToken = default);
        //Task ChangePasswordAsync(string userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
    }
}
