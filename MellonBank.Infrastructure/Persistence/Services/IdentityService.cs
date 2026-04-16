using MellonBank.Application.Common.Models;
using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Application.Models.MellonBank.Application.Models;
using MellonBank.Domain.Enums;
using MellonBank.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MellonBank.Infrastructure.Persistence.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserIdentityModel?> GetByAfmAsync(string afm, CancellationToken ct = default)
        {
            return await _userManager.Users
                .Where(u => u.Afm == afm)
                .Select(u => new UserIdentityModel(
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Address,
                    u.Afm,
                    u.PhoneNumber!,
                    u.Email!,
                    u.UserName!
                ))
                .SingleOrDefaultAsync(ct);
        }

        public async Task<IEnumerable<UserIdentityModel?>> GetUsersInRoleAsync(RoleType roleName, CancellationToken ct = default)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName.ToString());

            return users.OrderBy(u => u.FirstName).Select(user => new UserIdentityModel(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Address,
                user.Afm,
                user.PhoneNumber!,
                user.Email!,
                user.UserName!
            ));
        }

        public async Task<string?> CreateUserAsync(CreateUserRequestDto request, CancellationToken ct = default)
        {
            var user = new ApplicationUser(
                request.FirstName,
                request.LastName,
                request.Address,
                request.Afm
            );

            user.UserName = request.UserName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return null;

            return user.Id;
        }

        public async Task<bool> UpdateUserAsync(string afm, UpdateUserRequestDto request, CancellationToken ct = default)
        {
            var user = await FindApplicationUserByAfmAsync(afm, ct);
            if (user is null)
                return false;

            user.UpdateProfile(
                request.FirstName,
                request.LastName,
                request.Address,
                request.PhoneNumber
            );

            user.Email = request.Email;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string afm, CancellationToken ct = default)
        {
            var user = await FindApplicationUserByAfmAsync(afm, ct);
            if (user is null)
                return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
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

        public async Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure("User not found.");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                var errMessage = result.Errors.FirstOrDefault()?.Description ?? "Current password is incorrect or the new password is invalid.";

                return Result.Failure(errMessage);
            }

            return Result.Success();
        }

        private async Task<ApplicationUser?> FindApplicationUserByAfmAsync(string afm, CancellationToken ct = default)
        {
            return await _userManager.Users
                .SingleOrDefaultAsync(u => u.Afm == afm, ct);
        }
    }
}