using MellonBank.Application.Common.Models;
using MellonBank.Application.DTOs.Requests;

namespace MellonBank.Application.Interfaces.Services
{
    public interface ITransferService
    {
        Task<Result> TransferToOwnAccountAsync(TransferToOwnAccountRequestDto request, CancellationToken ct = default);
        Task<Result> TransferToThirdPartyAsync(TransferToThirdPartyRequestDto request, CancellationToken ct = default);
    }
}
