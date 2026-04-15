using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.DTOs.Responses;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Domain.Entities;
using MellonBank.Domain.Enums;
using MellonBank.Web.Areas.Staff.ViewModels.Accounts;
using MellonBank.Web.Areas.Staff.ViewModels.Customers;
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
        public async Task<IActionResult> Index()
        {
            var accounts = await _accountManagementService.GetAllAsync();
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
        public async Task<IActionResult> Create(CreateBankAccountViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _accountManagementService.CreateAccountAsync(
                    new CreateBankAccountRequestDto(
                        model.CustomerAfm,
                        model.AccountNumber,
                        model.InitialBalance,
                        CurrencyType.EUR,
                        model.Branch,
                        model.AccountType
                    ));

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating bank account.");

                ModelState.AddModelError(string.Empty, "An error occurred while creating the account.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest();

            var account = await _accountManagementService.GetByAccountNumberAsync(accountNumber);

            if (account is null)
                return NotFound();

            return View(MapToViewModel(account));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest();

            var account = await _accountManagementService.GetByAccountNumberAsync(accountNumber);

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
        public async Task<IActionResult> Edit(UpdateAccountViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _accountManagementService.UpdateAccountAsync(model.AccountNumber,
                    new UpdateBankAccountRequestDto(
                        model.Branch,
                        model.AccountType
                    ));

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating bank account {AccountNumber}", model.AccountNumber);

                ModelState.AddModelError(string.Empty, "An error occurred while updating the account.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest();

            var account = await _accountManagementService.GetByAccountNumberAsync(accountNumber);

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
        public async Task<IActionResult> DeleteConfirmed(DeleteAccountViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Delete", model);

            try
            {
                await _accountManagementService.DeleteAccountAsync(model.AccountNumber);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting bank account {AccountNumber}", model.AccountNumber);

                ModelState.AddModelError(string.Empty, "An error occurred while deleting the account.");

                return View("Delete", model);
            }
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
