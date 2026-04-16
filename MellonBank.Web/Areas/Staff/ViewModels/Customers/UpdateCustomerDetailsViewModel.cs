using MellonBank.Application.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace MellonBank.Web.Areas.Staff.ViewModels.Customers
{
    public class UpdateCustomerDetailsViewModel
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(16)]
        [RegularExpression(ValidationPatterns.PhoneNumber,
            ErrorMessage = ValidationMessages.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Afm { get; set; } = string.Empty;
    }
}
