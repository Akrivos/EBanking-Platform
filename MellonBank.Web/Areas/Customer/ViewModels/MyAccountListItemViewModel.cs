using MellonBank.Domain.Enums;

namespace MellonBank.Web.Areas.Customer.ViewModels
{
    public class MyAccountListItemViewModel
    {
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public CurrencyType Currency { get; set; }
        public string Branch { get; set; } = string.Empty;
        public AccountType AccountType { get; set; }
    }
}
