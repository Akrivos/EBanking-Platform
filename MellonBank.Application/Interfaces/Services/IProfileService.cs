using MellonBank.Application.Common.Models;
using MellonBank.Application.DTOs.Requests;

namespace MellonBank.Application.Interfaces.Services
{
    public interface IProfileService
    {
        Task<Result> ChangePasswordAsync(ChangePasswordRequestDto request, CancellationToken ct = default);
    }
}
