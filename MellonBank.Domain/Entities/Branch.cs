using MellonBank.Domain.Common;

namespace MellonBank.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }

        private Branch() { }

        public Branch(string code, string name, string address, string city)
        {
            SetBranchInfo(code, name, address, city);
            IsActive = true;
        }

        public void SetBranchInfo(string code, string name, string address, string city)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Code is required.", nameof(code));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address is required.", nameof(address));

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City is required.", nameof(city));

            Code = code.Trim();
            Name = name.Trim();
            Address = address.Trim();
            City = city.Trim();
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}
