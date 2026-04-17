using MellonBank.Application.Interfaces.Services;
using MellonBank.Domain.Enums;
using MellonBank.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace MellonBank.Infrastructure.Persistence.Services
{
    public sealed class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> RoleExistsAsync(RoleType roleName, CancellationToken ct = default)
        {
            return await _roleManager.RoleExistsAsync(roleName.ToString());
        }

        public async Task<bool> IsInRoleAsync(string userId, RoleType roleName, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return false;

            return await _userManager.IsInRoleAsync(user, roleName.ToString());
        }

        public async Task<bool> AddToRoleAsync(string userId, RoleType roleName, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return false;

            var role = roleName.ToString();

            if (await _userManager.IsInRoleAsync(user, role))
                return true;

            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }
    }
}
