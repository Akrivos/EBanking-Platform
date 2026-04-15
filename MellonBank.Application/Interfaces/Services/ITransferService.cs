using MellonBank.Application.DTOs.Requests;

namespace MellonBank.Application.Interfaces.Services
{
    public interface ITransferService
    {
        Task TransferToOwnAccountAsync(TransferToOwnAccountRequestDto request, CancellationToken ct = default);
        Task TransferToThirdPartyAsync(TransferToThirdPartyRequestDto request, CancellationToken ct = default);
    }
}
