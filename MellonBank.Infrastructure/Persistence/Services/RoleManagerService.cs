using MellonBank.Application.Interfaces.Services;
using MellonBank.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace MellonBank.Infrastructure.Persistence.Services
{
    public class RoleManagerService : IRoleManagerService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagerService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> RoleExistsAsync(RoleType roleName, CancellationToken ct = default)
        {
            return await _roleManager.RoleExistsAsync(roleName.ToString());
        }
    }
}
