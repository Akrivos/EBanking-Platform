using MellonBank.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MellonBank.Web.Areas.Staff.ViewModels.Accounts
{
    public class UpdateAccountViewModel
    {
        [Required]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Branch { get; set; } = string.Empty;

        [Required]
        public AccountType AccountType { get; set; } 
    }
}
