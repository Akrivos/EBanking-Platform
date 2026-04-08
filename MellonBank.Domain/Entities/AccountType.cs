using MellonBank.Domain.Common;

namespace MellonBank.Domain.Entities
{
    public class AccountType : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;

        private AccountType() { }

        public AccountType(string name, string description)
        {
            SetAccountTypeInfo(name, description);
        }

        public void SetAccountTypeInfo(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required.", nameof(description));

            Name = name.Trim();
            Description = description.Trim();
        }
    }
}
