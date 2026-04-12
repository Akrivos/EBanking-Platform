namespace MellonBank.Application.DTOs.Requests
{
    public record TransferToThirdPartyRequestDto(
        string FromAccountNumber,
        string ToAccountNumber,
        decimal Amount
    );
}
