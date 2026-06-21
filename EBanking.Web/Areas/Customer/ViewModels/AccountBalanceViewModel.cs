namespace EBanking.Web.Areas.Customer.ViewModels
{
    public class AccountBalanceViewModel
    {
        public string AccountNumber { get; set; } = string.Empty;
        public decimal BalanceEuro { get; set; }
        public decimal ExchangeRateUsd { get; set; }
        public decimal BalanceUsd { get; set; }
        public string Branch { get; set; } = string.Empty;
    }
}
