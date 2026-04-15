using MellonBank.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MellonBank.Web.Areas.Staff.ViewModels.Accounts
{
    public class CreateBankAccountViewModel
    {
        [Required]
        [StringLength(9)]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Customer AFM must be exactly 9 digits.")]
        public string CustomerAfm { get; set; } = string.Empty;

        [Required]
        [StringLength(20, ErrorMessage = "Account number cannot exceed 20 characters.")]
        public string AccountNumber { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Initial balance cannot be negative.")]
        public decimal InitialBalance { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Branch cannot exceed 100 characters.")]
        public string Branch { get; set; } = string.Empty;

        [Required]
        public AccountType AccountType { get; set; }
    }
}