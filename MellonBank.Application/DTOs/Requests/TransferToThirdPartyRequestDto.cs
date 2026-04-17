namespace MellonBank.Application.DTOs.Requests
{
    public sealed record TransferToThirdPartyRequestDto(
        string FromAccountNumber,
        string ToAccountNumber,
        decimal Amount
    );
}
