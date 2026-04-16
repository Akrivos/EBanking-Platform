using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Domain.Enums;
using MellonBank.Web.Areas.Staff.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MellonBank.Web.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles = "Staff")]
    public class UsersController : Controller
    {
        private readonly IStaffUserManagementService _staffUserManagementService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IStaffUserManagementService staffUserManagementService,
            ILogger<UsersController> logger)
        {
            _staffUserManagementService = staffUserManagementService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer(CreateCustomerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _staffUserManagementService.CreateCustomerAsync(
                    new CreateUserRequestDto(
                        model.FirstName,
                        model.LastName,
                        model.Afm,
                        model.Address,
                        model.PhoneNumber,
                        model.Email,
                        model.Username,
                        model.Password,
                        RoleType.Customer
                    ),ct);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, result.Error!);
                    return View(model);
                }

                TempData["SuccessMessage"] = "Customer created successfully.";

                return RedirectToAction("Index", "Customers", new { area = "Staff" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer");

                ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the customer.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult CreateStaff()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStaff(CreateStaffViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _staffUserManagementService.CreateStaffAsync(
                    new CreateUserRequestDto(
                        model.FirstName,
                        model.LastName,
                        model.Afm,
                        model.Address,
                        model.PhoneNumber,
                        model.Email,
                        model.Username,
                        model.Password,
                        RoleType.Staff
                    ), ct);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, result.Error!);
                    return View(model);
                }

                TempData["SuccessMessage"] = "Staff user created successfully.";

                return RedirectToAction("Index", "Dashboard", new { area = "Staff" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating staff user");

                ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the staff user.");

                return View(model);
            }
        }
    }
}