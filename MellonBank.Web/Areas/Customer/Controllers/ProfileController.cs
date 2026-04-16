using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Web.Areas.Customer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MellonBank.Web.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize(Roles = "Customer")]
public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;
    private readonly IProfileService _profileService;

    public ProfileController(
        ILogger<ProfileController> logger,
        IProfileService profileService)
    {
        _logger = logger;
        _profileService = profileService;
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await _profileService.ChangePasswordAsync(new ChangePasswordRequestDto(
                model.CurrentPassword,
                model.NewPassword,
                model.ConfirmPassword), ct);

            TempData["SuccessMessage"] = "Password changed successfully!";

            return RedirectToAction("Index", "Dashboard", new { area = "Customer" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while changing password.");

            ModelState.AddModelError(string.Empty, "An error occurred while changing the password.");
            return View(model);
        }
    }
}