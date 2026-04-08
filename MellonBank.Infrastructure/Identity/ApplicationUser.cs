using Microsoft.AspNetCore.Identity;

namespace MellonBank.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        private ApplicationUser() { }

        public ApplicationUser(string firstName, string lastName, string address, string afm)
        {
            SetUserInfo(firstName, lastName, address, afm);
        }

        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string Afm { get; private set; } = string.Empty;

        public void SetUserInfo(string firstName, string lastName, string address, string afm)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.", nameof(lastName));

            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address is required.", nameof(address));

            afm = afm?.Trim() ?? string.Empty;

            if (afm.Length != 9 || !afm.All(char.IsDigit))
                throw new ArgumentException("Afm must be exactly 9 digits.", nameof(afm));

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Address = address.Trim();
            Afm = afm;
        }
    }
}