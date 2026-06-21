using EBanking.Domain.Common;
using EBanking.Domain.Enums;
using EBanking.Domain.Exceptions;

namespace EBanking.Domain.Entities
{
    public class BankAccount : BaseEntity
    {
        public string AccountNumber { get; private set; } = string.Empty;
        public decimal Balance { get; private set; }
        public CurrencyType Currency { get; private set; }        public string Branch { get; private set; } = string.Empty;
        public AccountType AccountType { get; private set; }
        public string UserId { get; private set; } = string.Empty;
        public bool IsActive { get; private set; } = true;

        private BankAccount() { }

        public BankAccount(
            string accountNumber,
            decimal balance,
            CurrencyType currency,
            string userId,
            string branch,
            AccountType accountType)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User id is required.", nameof(userId));

            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number is required.", nameof(accountNumber));

            if (balance < 0)
                throw new ArgumentException("Balance cannot be negative.", nameof(balance));

            if (!Enum.IsDefined(typeof(CurrencyType), currency))
                throw new ArgumentException("Invalid currency.", nameof(currency));

            if (string.IsNullOrWhiteSpace(branch))
                throw new ArgumentException("Branch is required.", nameof(branch));

            if (!Enum.IsDefined(typeof(AccountType), accountType))
                throw new ArgumentException("Invalid account type.", nameof(accountType));

            AccountNumber = accountNumber.Trim();
            Balance = balance;
            Currency = currency;
            UserId = userId.Trim();
            Branch = branch.Trim();
            AccountType = accountType;
            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("Account is already inactive.");

            IsActive = false;
        }

        public void Activate()
        {
            if (IsActive)
                throw new InvalidOperationException("Account is already active.");

            IsActive = true;
        }

        public void Credit(decimal amount)
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot credit an inactive account.");


            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            Balance += amount;
        }

        public void Debit(decimal amount)
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot debit an inactive account.");

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            if (Balance < amount)
                throw new InsufficientBalanceException();

            Balance -= amount;
        }

        public void UpdateDetails(string branch, AccountType accountType)
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot update an inactive account.");

            if (string.IsNullOrWhiteSpace(branch))
                throw new ArgumentException("Branch is required.", nameof(branch));

            if (!Enum.IsDefined(typeof(AccountType), accountType))
                throw new ArgumentException("Invalid account type.", nameof(accountType));

            Branch = branch.Trim();
            AccountType = accountType;
        }
    }
}