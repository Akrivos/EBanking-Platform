using MellonBank.Domain.Common;
using MellonBank.Domain.Enums;
using MellonBank.Domain.Exceptions;

namespace MellonBank.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid FromAccountId { get; private set; }
        public BankAccount FromAccount { get; private set; } = default!;
        public Guid ToAccountId { get; private set; }
        public BankAccount ToAccount { get; private set; } = default!;
        public TransactionType Type { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public TransactionStatus Status { get; private set; }
        public string ExecutedByUserId { get; private set; } = string.Empty;
        public string ReferenceCode { get; private set; } = string.Empty;

        private Transaction() { }

        private Transaction(
            Guid fromAccountId,
            Guid toAccountId,
            TransactionType type,
            decimal amount,
            string? description,
            string executedByUserId)
        {
            if (fromAccountId == Guid.Empty)
                throw new ArgumentException("From account id is required.", nameof(fromAccountId));

            if (toAccountId == Guid.Empty)
                throw new ArgumentException("To account id is required.", nameof(toAccountId));

            if (fromAccountId == toAccountId)
                throw new InvalidAccountOperationException("Source and destination account cannot be the same.");

            if (!Enum.IsDefined(typeof(TransactionType), type))
                throw new ArgumentException("Invalid transaction type.", nameof(type));

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            if (string.IsNullOrWhiteSpace(executedByUserId))
                throw new ArgumentException("Executed by user id is required.", nameof(executedByUserId));

            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Type = type;
            Amount = amount;
            Description = description?.Trim() ?? string.Empty;
            ExecutedByUserId = executedByUserId.Trim();
            Status = TransactionStatus.Pending;
            ReferenceCode = GenerateReferenceCode();
        }

        public static Transaction CreateOwnTransfer(
            Guid fromAccountId,
            Guid toAccountId,
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
            Guid fromAccountId,
            Guid toAccountId,
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
                throw new BusinessRuleException("Only pending transactions can be completed.");

            Status = TransactionStatus.Completed;
        }

        public void MarkFailed()
        {
            if (Status != TransactionStatus.Pending)
                throw new BusinessRuleException("Only pending transactions can be failed.");

            Status = TransactionStatus.Failed;
        }

        private static string GenerateReferenceCode()
        {
            return $"TXN-{Guid.NewGuid():N}"[..12].ToUpper();
        }
    }
}