using EBanking.Domain.Enums;

namespace EBanking.Web.Areas.Customer.ViewModels;

public sealed class TransactionListItemViewModel
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string ReferenceCode { get; set; } = string.Empty;
    public string FromAccountNumber { get; set; } = string.Empty;
    public string ToAccountNumber { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsIncoming { get; set; }
    public string AmountPrefix => IsIncoming ? "+" : "-";
    public string AmountCssClass => IsIncoming ? "text-success" : "text-danger";
    public string DirectionText =>
        IsIncoming
            ? "Incoming transaction"
            : "Outgoing transaction";
    public string DirectionBadgeCssClass =>
        IsIncoming
            ? "bg-success"
            : "bg-danger";
}