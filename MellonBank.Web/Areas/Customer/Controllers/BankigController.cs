using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Web.Areas.Customer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MellonBank.Web.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize(Roles = "Customer")]
public class BankingController : Controller
{
    private readonly ICustomerAccountService _customerAccountService;
    private readonly ITransferService _transferService;
    private readonly ILogger<BankingController> _logger;

    public BankingController(
        ICustomerAccountService customerAccountService,
        ITransferService transferService,
        ILogger<BankingController> logger
    )
    {
        _customerAccountService = customerAccountService;
        _transferService = transferService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> MyAccounts(CancellationToken ct)
    {
        var myAccounts = await _customerAccountService.GetAccountsAsync(ct);

        var myAccountListViewModel = myAccounts.Select(a => new MyAccountListItemViewModel
        {
            AccountNumber = a.AccountNumber,
            Balance = a.Balance,
            Branch = a.Branch,
            AccountType = a.AccountType
        }).ToList();

        return View(myAccountListViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> AccountDetails(string? accountNumber, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            return BadRequest();

        var account = await _customerAccountService.GetAccountDetailsAsync(accountNumber, ct);

        if (account is null)
            return NotFound();

        var model = new MyAccountListItemViewModel
        {
            AccountNumber = account.AccountNumber,
            Balance = account.Balance,
            Currency = account.Currency,
            Branch = account.Branch,
            AccountType = account.AccountType
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> TransferToOwn(CancellationToken ct, string? accountNumber = null)
    {
        var accounts = (await _customerAccountService.GetAccountsAsync(ct)).ToList();

        if (accounts.Count < 2)
        {
            TempData["InfoMessage"] = "You need at least two accounts to transfer money between your own accounts.";
            return RedirectToAction(nameof(MyAccounts));
        }

        var model = new TransferToOwnViewModel
        {
            FromAccountNumber = accountNumber ?? string.Empty,
            AvailableAccounts = accounts.Select(a => new SelectListItem
            {
                Value = a.AccountNumber,
                Text = $"{a.AccountNumber} - {a.AccountType} - {a.Balance:N2} €"
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TransferToOwn(TransferToOwnViewModel model, CancellationToken ct)
    {
        var accounts = (await _customerAccountService.GetAccountsAsync(ct)).ToList();

        model.AvailableAccounts = accounts.Select(a => new SelectListItem
        {
            Value = a.AccountNumber,
            Text = $"{a.AccountNumber} - {a.AccountType} - {a.Balance:N2} €"
        }).ToList();

        if (model.FromAccountNumber == model.ToAccountNumber)
        {
            ModelState.AddModelError(string.Empty, "Source and destination account must be different.");
        }

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var result = await _transferService.TransferToOwnAccountAsync(
                new TransferToOwnAccountRequestDto(
                    FromAccountNumber: model.FromAccountNumber,
                    ToAccountNumber: model.ToAccountNumber,
                    Amount: model.Amount
                ),
                ct);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                return View(model);
            }


            TempData["SuccessMessage"] = "Transfer completed successfully.";
            return RedirectToAction(nameof(MyAccounts));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during transfer between own accounts.");

            ModelState.AddModelError(string.Empty, "An unexpected error occurred while processing the transfer.");

            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> TransferToThirdParty(CancellationToken ct, string? accountNumber = null)
    {
        var accounts = (await _customerAccountService.GetAccountsAsync(ct)).ToList();

        if (!accounts.Any())
        {
            TempData["InfoMessage"] = "You do not have any accounts.";
            return RedirectToAction(nameof(MyAccounts));
        }

        var model = new TransferToThirdPartyViewModel
        {
            FromAccountNumber = accountNumber ?? string.Empty,
            AvailableAccounts = accounts.Select(a => new SelectListItem
            {
                Value = a.AccountNumber,
                Text = $"{a.AccountNumber} - {a.AccountType} - {a.Balance:N2} €"
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TransferToThirdParty(TransferToThirdPartyViewModel model, CancellationToken ct)
    {
        var accounts = (await _customerAccountService.GetAccountsAsync(ct)).ToList();

        model.AvailableAccounts = accounts.Select(a => new SelectListItem
        {
            Value = a.AccountNumber,
            Text = $"{a.AccountNumber} - {a.AccountType} - {a.Balance:N2} €"
        }).ToList();


        if (model.FromAccountNumber == model.ToAccountNumber)
        {
            ModelState.AddModelError(string.Empty, "Source and destination account must be different.");
        }

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var result = await _transferService.TransferToThirdPartyAsync(
                new TransferToThirdPartyRequestDto(
                    model.FromAccountNumber,
                    model.ToAccountNumber,
                    model.Amount
                ),
                ct);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                return View(model);
            }

            TempData["SuccessMessage"] = "Transfer completed successfully.";
            return RedirectToAction(nameof(MyAccounts));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during third-party transfer.");

            ModelState.AddModelError(string.Empty, "An unexpected error occurred during the transfer.");
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Balance(string? accountNumber, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            var accounts = (await _customerAccountService.GetAccountsAsync(ct)).ToList();

            if (!accounts.Any())
            {
                TempData["InfoMessage"] = "You do not have any bank accounts yet.";
                return RedirectToAction(nameof(MyAccounts));
            }

            if (accounts.Count == 1)
            {
                return RedirectToAction(nameof(Balance), new
                {
                    accountNumber = accounts[0].AccountNumber
                });
            }

            var selectionModel = accounts.Select(a => new MyAccountListItemViewModel
            {
                AccountNumber = a.AccountNumber,
                Balance = a.Balance,
                Branch = a.Branch,
                AccountType = a.AccountType
            }).ToList();

            return View("SelectAccountForBalance", selectionModel);
        }

        try
        {
            var account = await _customerAccountService.GetAccountDetailsAsync(accountNumber, ct);

            if (account is null)
                return NotFound();

            var convertedBalance = await _customerAccountService.GetConvertedBalanceAsync(accountNumber, ct);

            if (convertedBalance is null)
            {
                TempData["InfoMessage"] = "Unable to retrieve conversion rate at the moment.";
                return RedirectToAction(nameof(MyAccounts));
            }

            var model = new AccountBalanceViewModel
            {
                AccountNumber = account.AccountNumber,
                Branch = account.Branch,
                BalanceEuro = account.Balance,
                ExchangeRateUsd = convertedBalance.ConversionRate,
                BalanceUsd = convertedBalance.ConversionResult
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error while retrieving balance for account {AccountNumber}.",
                accountNumber);

            TempData["InfoMessage"] = "Unable to retrieve balance information at the moment.";

            return RedirectToAction(nameof(MyAccounts));
        }
    }

}