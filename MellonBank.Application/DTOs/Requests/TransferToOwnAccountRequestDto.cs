namespace MellonBank.Application.DTOs.Requests
{
    public record TransferToOwnAccountRequestDto(
        string FromAccountNumber,
        string ToAccountNumber,
        decimal Amount
    );
}
