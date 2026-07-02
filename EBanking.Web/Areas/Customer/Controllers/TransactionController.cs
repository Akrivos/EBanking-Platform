using EBanking.Application.Interfaces.Services;
using EBanking.Web.Areas.Customer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBanking.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public sealed class TransactionsController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string accountNumber,
            int page = 1,
            int pageSize = 10,
            CancellationToken ct = default)
        {
            var result = await _transactionService
                .GetAccountTransactionsAsync(accountNumber, page, pageSize, ct);

            var viewModel = new TransactionsIndexViewModel
            {
                AccountNumber = accountNumber,
                Page = result.Page,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
                HasPreviousPage = result.HasPreviousPage,
                HasNextPage = result.HasNextPage,
                Transactions = result.Items.Select(t =>
                    new TransactionListItemViewModel
                    {
                        Id = t.Id,
                        AccountNumber = accountNumber,
                        ReferenceCode = t.ReferenceCode,
                        FromAccountNumber = t.FromAccountNumber,
                        ToAccountNumber = t.ToAccountNumber,
                        Type = t.Type,
                        Status = t.Status,
                        Amount = t.Amount,
                        Description = t.Description,
                        CreatedAt = t.CreatedAt,

                        IsIncoming =
                            t.ToAccountNumber == accountNumber
                    }).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(
            Guid id,
            string accountNumber,
            CancellationToken ct = default)
        {
            var transaction = await _transactionService
                .GetTransactionDetailsAsync(id, ct);

            var viewModel = new TransactionListItemViewModel
            {
                Id = transaction.Id,
                AccountNumber = accountNumber,
                ReferenceCode = transaction.ReferenceCode,
                FromAccountNumber = transaction.FromAccountNumber,
                ToAccountNumber = transaction.ToAccountNumber,
                Type = transaction.Type,
                Status = transaction.Status,
                Amount = transaction.Amount,
                Description = transaction.Description,
                CreatedAt = transaction.CreatedAt,
                IsIncoming = transaction.ToAccountNumber == accountNumber
            };

            return View(viewModel);
        }
    }
}