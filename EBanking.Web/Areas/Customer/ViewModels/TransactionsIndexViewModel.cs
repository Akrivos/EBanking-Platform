namespace EBanking.Web.Areas.Customer.ViewModels
{
    public sealed class TransactionsIndexViewModel
    {
        public string AccountNumber { get; set; } = string.Empty;
        public IReadOnlyList<TransactionListItemViewModel> Transactions { get; set; } = new List<TransactionListItemViewModel>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
