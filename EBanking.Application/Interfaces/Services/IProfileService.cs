using EBanking.Application.Common.Models;
using EBanking.Application.DTOs.Requests;

namespace EBanking.Application.Interfaces.Services
{
    public interface IProfileService
    {
        Task<Result> ChangePasswordAsync(ChangePasswordRequestDto request, CancellationToken ct = default);
    }
}
