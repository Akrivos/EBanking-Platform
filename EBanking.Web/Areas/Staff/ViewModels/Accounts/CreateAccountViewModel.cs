using EBanking.Application.Common.Validation;
using EBanking.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace EBanking.Web.Areas.Staff.ViewModels.Accounts
{
    public class CreateBankAccountViewModel
    {
        [Required]
        [StringLength(9)]
        [RegularExpression(ValidationPatterns.Afm, ErrorMessage = ValidationMessages.Afm)]
        public string CustomerAfm { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Initial balance cannot be negative.")]
        public decimal InitialBalance { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Branch cannot exceed 100 characters.")]
        public string Branch { get; set; } = string.Empty;

        [Required]
        public AccountType AccountType { get; set; }
    }
}