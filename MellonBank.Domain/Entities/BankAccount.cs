using MellonBank.Domain.Common;

namespace MellonBank.Domain.Entities
{
    public class BankAccount : BaseEntity  
    {
        public string AccountNumber { get; private set; } = string.Empty;
        public decimal Balance { get; private set; } = 0m;
        public string Currency { get; private set; } = "EUR";
        public bool IsActive { get; private set; } = true;
        public Guid BranchId { get; private set; }
        public Branch Branch { get; private set; } = default!;
        public Guid AccountTypeId { get; private set; }
        public AccountType AccountType { get; private set; } = default!;
        public string UserId { get; private set; }

        private BankAccount() { }

        public BankAccount(string accountNumber, decimal balance, string currency, Guid branchId, Guid accountTypeId, string userId)
        {
            SetBankAccountInfo(accountNumber, balance, currency, branchId, accountTypeId, userId);
        }

        public void SetBankAccountInfo(string accountNumber, decimal balance, string currency, Guid branchId, Guid accountTypeId, string userId)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number is required.", nameof(accountNumber));
            if (balance < 0)
                throw new ArgumentException("Balance cannot be negative.", nameof(balance));
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency is required.", nameof(currency));
            AccountNumber = accountNumber.Trim();
            Balance = balance;
            Currency = currency.Trim();
            BranchId = branchId;
            AccountTypeId = accountTypeId;
            UserId = userId;
        }

    }
}
