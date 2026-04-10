using MellonBank.Domain.Common;
using MellonBank.Domain.Enums;

namespace MellonBank.Domain.Entities
{
    public class BankAccount : BaseEntity  
    {
        public string AccountNumber { get; private set; } = string.Empty;
        public decimal Balance { get; private set; } = 0m;
        public CurrencyType Currency { get; private set; }
        public bool IsActive { get; private set; } = true;
        public string Branch { get; private set; } = string.Empty;
        public AccountType AccountType { get; private set; }
        public string UserId { get; private set; } = string.Empty;


        private BankAccount() { }

        public BankAccount(
            string accountNumber, 
            decimal balance, 
            CurrencyType currency, 
            string userId,
            string branch,
            AccountType accountType
            )
        {
            SetBankAccountInfo(accountNumber, balance, currency, userId, branch, accountType);
        }

        public void SetBankAccountInfo(
            string accountNumber, 
            decimal balance, 
            CurrencyType currency, 
            string userId , 
            string branch,
            AccountType accountType
            )
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
            UserId = userId;
            Branch = branch.Trim();
            AccountType = accountType;
        }

    }
}
