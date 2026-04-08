using MellonBank.Domain.Common;
using MellonBank.Domain.Enums;

namespace MellonBank.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public int FromAccountId { get; private set; }
        public BankAccount FromAccount { get; private set; } = default!;

        public int ToAccountId { get; private set; }
        public BankAccount ToAccount { get; private set; } = default!;

        public TransactionType Type { get; private set; }
        public decimal Amount { get; private set; } = 0m;
        public string Description { get; private set; } = string.Empty;
        public TransactionStatus Status { get; private set; } 
        public string ExecutedByUserId { get; private set; } = string.Empty;
        public string ReferenceCode { get; private set; } = string.Empty;

        private Transaction() { }

        private Transaction(
            int fromAccountId,
            int toAccountId,
            TransactionType type,
            decimal amount,
            string? description,
            string executedByUserId)
        {
            if (fromAccountId <= 0)
                throw new ArgumentException("From account id is required.", nameof(fromAccountId));

            if (toAccountId <= 0)
                throw new ArgumentException("To account id is required.", nameof(toAccountId));

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            if (fromAccountId == toAccountId && type == TransactionType.ThirdPartyTransfer)
                throw new ArgumentException("Third-party transfer cannot target the same account.");

            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Type = type;
            Amount = amount;
            Description = description?.Trim() ?? string.Empty;
            ExecutedByUserId = executedByUserId;
            Status = TransactionStatus.Pending;
            ReferenceCode = GenerateReferenceCode();
        }

        public static Transaction CreateOwnTransfer(
            int fromAccountId,
            int toAccountId,
            decimal amount,
            string? description,
            string executedByUserId)
        {
            return new Transaction(
                fromAccountId,
                toAccountId,
                TransactionType.OwnAccountTransfer,
                amount,
                description,
                executedByUserId);
        }

        public static Transaction CreateThirdPartyTransfer(
            int fromAccountId,
            int toAccountId,
            decimal amount,
            string? description,
            string executedByUserId)
        {
            return new Transaction(
                fromAccountId,
                toAccountId,
                TransactionType.ThirdPartyTransfer,
                amount,
                description,
                executedByUserId);
        }

        public void MarkCompleted()
        {
            if (Status != TransactionStatus.Pending)
                throw new InvalidOperationException("Only pending transactions can be completed.");

            Status = TransactionStatus.Completed;
        }

        public void MarkFailed()
        {
            if (Status != TransactionStatus.Pending)
                throw new InvalidOperationException("Only pending transactions can be failed.");

            Status = TransactionStatus.Failed;
        }

        private static string GenerateReferenceCode()
        {
            return $"TXN-{Guid.NewGuid():N}"[..12].ToUpper();
        }
    }
}