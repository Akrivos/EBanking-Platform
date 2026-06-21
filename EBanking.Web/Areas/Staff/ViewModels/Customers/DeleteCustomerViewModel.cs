using System.ComponentModel.DataAnnotations;

namespace EBanking.Web.Areas.Staff.ViewModels.Customers
{
    public class DeleteCustomerViewModel
    {
        [Required]
        public string Afm { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }

}
