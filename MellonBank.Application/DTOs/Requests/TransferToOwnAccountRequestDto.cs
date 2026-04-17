namespace MellonBank.Application.DTOs.Requests
{
    public sealed record TransferToOwnAccountRequestDto(
        string FromAccountNumber,
        string ToAccountNumber,
        decimal Amount
    );
}
