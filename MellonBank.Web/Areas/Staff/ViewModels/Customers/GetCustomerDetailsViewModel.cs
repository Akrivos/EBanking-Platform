using MellonBank.Domain.Enums;

namespace MellonBank.Web.Areas.Staff.ViewModels.Customers
{
    public class GetCustomerDetailsViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Afm { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Username { get; set; }

        public RoleType Role { get; set; }
    }
}
