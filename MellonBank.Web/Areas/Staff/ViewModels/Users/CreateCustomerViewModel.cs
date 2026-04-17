using MellonBank.Application.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace MellonBank.Web.Areas.Staff.ViewModels.Users
{
    public class CreateCustomerViewModel
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
        [StringLength(9)]
        [RegularExpression(ValidationPatterns.Afm, ErrorMessage = ValidationMessages.Afm)]
        public string Afm { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        [RegularExpression(ValidationPatterns.PhoneNumber,
            ErrorMessage = ValidationMessages.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [RegularExpression(ValidationPatterns.StrongPassword,
            ErrorMessage = ValidationMessages.StrongPassword)]
        public string Password { get; set; } = string.Empty;
    }
}
