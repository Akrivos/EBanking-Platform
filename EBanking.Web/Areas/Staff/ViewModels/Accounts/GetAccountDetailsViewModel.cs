using EBanking.Domain.Enums;

namespace EBanking.Web.Areas.Staff.ViewModels.Accounts
{
    public class GetAccountDetailsViewModel
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string CustomerAfm { get; set; } = string.Empty;
        public decimal Balance { get; set; } = 0;
        public CurrencyType Currency { get; set; }
        public string Branch { get; set; } = string.Empty;
        public AccountType AccountType { get; set; }
    }
}
