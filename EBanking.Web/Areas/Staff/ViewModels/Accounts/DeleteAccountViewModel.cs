using EBanking.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace EBanking.Web.Areas.Staff.ViewModels.Accounts
{
    public class DeleteAccountViewModel
    {
        [Required]
        public string AccountNumber { get; set; } = string.Empty;

        public string CustomerAfm { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        public CurrencyType Currency { get; set; }

        public string Branch { get; set; } = string.Empty;

        public AccountType AccountType { get; set; }
    }
}
