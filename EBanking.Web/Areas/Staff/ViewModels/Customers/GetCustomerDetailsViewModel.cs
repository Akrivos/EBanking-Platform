using EBanking.Domain.Enums;

namespace EBanking.Web.Areas.Staff.ViewModels.Customers
{
    public class GetCustomerDetailsViewModel
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Afm { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;
    }
}
