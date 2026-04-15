using MellonBank.Domain.Enums;

namespace MellonBank.Web.Areas.Staff.ViewModels.Accounts
{
    public class GetAccountDetailsViewModel
    {
        public string AccountNumber { get; set; }
        public string CustomerAfm { get; set; }
        public decimal Balance { get; set; }
        public CurrencyType Currency { get; set; }
        public string Branch { get; set; }
        public AccountType AccountType { get; set; }
    }
}
