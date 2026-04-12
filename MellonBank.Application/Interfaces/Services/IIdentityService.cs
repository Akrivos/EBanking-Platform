using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.Models.MellonBank.Application.Models;

namespace MellonBank.Application.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<string> CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default);
        Task<bool> UpdateUserAsync(string afm, UpdateUserRequestDto request, CancellationToken cancellationToken = default);
        Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
        Task<UserIdentityModel> GetByAfmAsync(string afm, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserIdentityModel?>> GetUsersInRoleAsync(CancellationToken cancellationToken = default);
    }
}
