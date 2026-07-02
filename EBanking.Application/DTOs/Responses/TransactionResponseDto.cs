using EBanking.Domain.Enums;

namespace EBanking.Application.DTOs.Responses
{
    public sealed record TransactionResponseDto(
        Guid Id,
        string ReferenceCode,
        string FromAccountNumber,
        string ToAccountNumber,
        TransactionType Type,
        TransactionStatus Status,
        decimal Amount,
        string Description,
        DateTimeOffset CreatedAt
    );
}
