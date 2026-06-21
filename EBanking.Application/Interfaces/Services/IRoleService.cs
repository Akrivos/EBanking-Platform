using EBanking.Domain.Enums;

namespace EBanking.Application.Interfaces.Services
{
    public interface IRoleService
    {
        Task<bool> RoleExistsAsync(RoleType roleName, CancellationToken ct = default);
        //Task<bool> CreateRoleAsync(RoleType roleName, CancellationToken ct = default);
        Task<bool> AddToRoleAsync(string userId, RoleType roleName, CancellationToken ct = default);
        Task<bool> IsInRoleAsync(string userId, RoleType roleName, CancellationToken ct = default);
    }
}
