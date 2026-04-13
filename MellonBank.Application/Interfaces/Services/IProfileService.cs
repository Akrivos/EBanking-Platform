using MellonBank.Application.DTOs.Requests;

namespace MellonBank.Application.Interfaces.Services
{
    public interface IProfileService
    {
        Task<bool> ChangePasswordAsync(ChangePasswordRequestDto request, CancellationToken ct = default);
    }
}
