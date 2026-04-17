using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.DTOs.Responses;
using MellonBank.Application.Exceptions;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Web.Areas.Staff.ViewModels.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MellonBank.Web.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles = "Staff")]
    public class CustomersController : Controller
    {
        private readonly IStaffUserManagementService _staffUserManagementService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(
            IStaffUserManagementService staffUserManagementService,
            ILogger<CustomersController> logger)
        {
            _staffUserManagementService = staffUserManagementService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct, string? afm)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(afm))
                {
                    var customer = await _staffUserManagementService.GetCustomerByAfmAsync(afm, ct);

                    return View(new List<GetCustomerDetailsViewModel>
                    {
                        MapToViewModel(customer!)
                    });
                }

                var customers = await _staffUserManagementService.GetAllCustomersAsync(ct);
                var model = customers.Select(c => MapToViewModel(c!)).ToList();

                return View(model);
            }
            catch (AppNotFoundException)
            {
                ViewBag.Error = $"No customer was found with AFM {afm}.";
                return View(Enumerable.Empty<GetCustomerDetailsViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string afm, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(afm))
                return BadRequest();

            try
            {
                var customer = await _staffUserManagementService.GetCustomerByAfmAsync(afm, ct);
                return View(MapToViewModel(customer!));
            }
            catch (AppNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string afm, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(afm))
                return BadRequest();

            try
            {
                var customer = await _staffUserManagementService.GetCustomerByAfmAsync(afm, ct);

                var updateCustomerViewModel = new UpdateCustomerDetailsViewModel
                {
                    Afm = customer!.Afm,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Address = customer.Address,
                    PhoneNumber = customer.PhoneNumber,
                    Email = customer.Email,
                };

                return View(updateCustomerViewModel);
            }
            catch (AppNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCustomerDetailsViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _staffUserManagementService.UpdateCustomerAsync(
                model.Afm,
                new UpdateUserRequestDto(
                    model.FirstName,
                    model.LastName,
                    model.Address,
                    model.PhoneNumber,
                    model.Email), ct);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                return View(model);
            }

            TempData["SuccessMessage"] = "Customer updated successfully.";
            return RedirectToAction(nameof(Index), new { area = "Staff" });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string afm, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(afm))
                return BadRequest();

            try
            {
                var customer = await _staffUserManagementService.GetCustomerByAfmAsync(afm, ct);

                var model = new DeleteCustomerViewModel
                {
                    FirstName = customer!.FirstName,
                    LastName = customer.LastName,
                    Afm = customer.Afm,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber
                };

                return View(model);
            }
            catch (AppNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DeleteCustomerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View("Delete", model);

            var result = await _staffUserManagementService.DeleteCustomerAsync(model.Afm, ct);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                return View("Delete", model);
            }

            TempData["SuccessMessage"] = "Customer deleted successfully.";
            return RedirectToAction(nameof(Index), new { area = "Staff" });
        }

        [HttpGet]
        public IActionResult Search(string afm)
        {
            return View();
        }

        private static GetCustomerDetailsViewModel MapToViewModel(UserResponseDto customer)
        {
            return new GetCustomerDetailsViewModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Afm = customer.Afm,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Username = customer.UserName
            };
        }
    }
}