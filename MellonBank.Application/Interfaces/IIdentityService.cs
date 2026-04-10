namespace MellonBank.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<string> CreateCustomerAsync(CreateIdentityUserRequest request, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(UpdateIdentityUserRequest request, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
        Task<UserDto?> GetByAfmAsync(string afm, CancellationToken cancellationToken = default);
        Task<UserDto?> GetByIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<UserDto>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default);
        Task<bool> IsInRoleAsync(string userId, string roleName, CancellationToken cancellationToken = default);
        Task ChangePasswordAsync(string userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
    }
}
