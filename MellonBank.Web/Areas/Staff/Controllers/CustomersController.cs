using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.DTOs.Responses;
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

        public CustomersController(IStaffUserManagementService staffUserManagementService, ILogger<CustomersController> logger)
        {
            _staffUserManagementService = staffUserManagementService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? afm)
        {
            if (!string.IsNullOrWhiteSpace(afm))
            {
                var customer = await _staffUserManagementService.GetCustomerByAfmAsync(afm);

                if (customer is null)
                {
                    ViewBag.Error = "Customer not found.";
                    return View(Enumerable.Empty<GetCustomerDetailsViewModel>());
                }

                return View(new List<GetCustomerDetailsViewModel>
                {
                    MapToViewModel(customer)
                });
            }

            var customers = await _staffUserManagementService.GetAllCustomersAsync();

            var model = customers.Select(c => MapToViewModel(c!)).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string afm)
        {
            if (string.IsNullOrWhiteSpace(afm))
                return BadRequest();

            var customer = await _staffUserManagementService.GetCustomerByAfmAsync(afm);

            if (customer is null)
                return NotFound();

            return View(MapToViewModel(customer));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string afm)
        {
            if (string.IsNullOrWhiteSpace(afm))
                return BadRequest();

            var customer = await _staffUserManagementService.GetCustomerByAfmAsync(afm);

            if (customer is null)
                return NotFound();

            var updateCustomerViewModel = new UpdateCustomerDetailsViewModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
            };

            return View(updateCustomerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCustomerDetailsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _staffUserManagementService.UpdateCustomerAsync(model.Afm, new UpdateUserRequestDto(
                    model.FirstName,
                    model.LastName,
                    model.Address,
                    model.PhoneNumber,
                    model.Email
                ));

                return RedirectToAction("Index", "Customers", new { area = "Staff" });
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the customer.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string afm)
        {
            if (string.IsNullOrWhiteSpace(afm))
                return BadRequest();

            var customer = await _staffUserManagementService.GetCustomerByAfmAsync(afm);

            if (customer is null)
                return NotFound();

            var model = new DeleteCustomerViewModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Afm = customer.Afm,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DeleteCustomerViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Delete", model);

            try
            {
                await _staffUserManagementService.DeleteCustomerAsync(model.Afm);

                return RedirectToAction("Index", "Customers", new { area = "Staff" });
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the customer.");
                return View("Delete", model);
            }
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
