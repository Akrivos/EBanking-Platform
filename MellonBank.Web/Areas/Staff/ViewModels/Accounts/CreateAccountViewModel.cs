using MellonBank.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MellonBank.Web.Areas.Staff.ViewModels.Accounts
{
    public class CreateAccountViewModel
    {
        //[Required]
        //public string CustomerAfm { get; set; }

        [Required]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Initial balance must be a non-negative number.")]
        public decimal InitialBalance { get; set; } = 0;

        [Required]
        [EnumDataType(typeof(CurrencyType), ErrorMessage = "Invalid currency type.")]
        public CurrencyType Currency { get; set; } = CurrencyType.EUR;

        [Required]
        [StringLength(100)]
        public string Branch { get; set; }

        [Required]
        [EnumDataType(typeof(AccountType), ErrorMessage = "Invalid account type.")]
        public AccountType AccountType { get; set; }
    }
}
