using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.DTOs.Responses;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Domain.Enums;
using MellonBank.Web.Areas.Staff.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MellonBank.Web.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles = "Staff")]
    public class BankAccountsController : Controller
    {
        private readonly IAccountManagementService _accountManagementService;
        private readonly ILogger<BankAccountsController> _logger;

        public BankAccountsController(
            IAccountManagementService accountManagementService,
            ILogger<BankAccountsController> logger)
        {
            _accountManagementService = accountManagementService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var accounts = await _accountManagementService.GetAllAsync(ct);
            var model = accounts.Select(ac => MapToViewModel(ac)).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBankAccountViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountManagementService.CreateAccountAsync(
                new CreateBankAccountRequestDto(
                    model.CustomerAfm,
                    model.AccountNumber,
                    model.InitialBalance,
                    CurrencyType.EUR,
                    model.Branch,
                    model.AccountType
                ),ct);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                return View(model);
            }

            TempData["SuccessMessage"] = "Account created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string accountNumber, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest();

            var account = await _accountManagementService.GetByAccountNumberAsync(accountNumber, ct);

            if (account is null)
                return NotFound();

            return View(MapToViewModel(account));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string accountNumber, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest();

            var account = await _accountManagementService.GetByAccountNumberAsync(accountNumber, ct);

            if (account is null)
                return NotFound();

            var model = new UpdateAccountViewModel
            {
                AccountNumber = account.AccountNumber,
                Branch = account.Branch,
                AccountType = account.AccountType
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateAccountViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountManagementService.UpdateAccountAsync(
                model.AccountNumber,
                new UpdateBankAccountRequestDto(
                    model.Branch,
                    model.AccountType
                ), ct);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                return View(model);
            }

            TempData["SuccessMessage"] = "Account updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string accountNumber, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest();

            var account = await _accountManagementService.GetByAccountNumberAsync(accountNumber, ct);

            if (account is null)
                return NotFound();

            var model = new DeleteAccountViewModel
            {
                AccountNumber = account.AccountNumber,
                CustomerAfm = account.CustomerAfm!,
                Balance = account.Balance,
                Currency = account.Currency,
                Branch = account.Branch,
                AccountType = account.AccountType
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DeleteAccountViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View("Delete", model);

            var result = await _accountManagementService.DeleteAccountAsync(model.AccountNumber, ct);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                return View("Delete", model);
            }

            TempData["SuccessMessage"] = "Account deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private static GetAccountDetailsViewModel MapToViewModel(AccountDetailsResponseDto account)
        {
            return new GetAccountDetailsViewModel
            {
                AccountNumber = account.AccountNumber,
                Balance = account.Balance,
                Currency = account.Currency,
                Branch = account.Branch,
                AccountType = account.AccountType,
                CustomerAfm = account.CustomerAfm
            };
        }
    }
}
