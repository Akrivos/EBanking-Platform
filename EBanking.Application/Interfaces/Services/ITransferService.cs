using EBanking.Application.Common.Models;
using EBanking.Application.DTOs.Requests;

namespace EBanking.Application.Interfaces.Services
{
    public interface ITransferService
    {
        Task<Result> TransferToOwnAccountAsync(TransferToOwnAccountRequestDto request, CancellationToken ct = default);
        Task<Result> TransferToThirdPartyAsync(TransferToThirdPartyRequestDto request, CancellationToken ct = default);
    }
}
