using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MellonBank.Web.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize(Roles = "Customer")]
public class BankingController : Controller
{
    [HttpGet]
    public IActionResult MyAccounts()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Balance()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AccountDetails()
    {
        return View();
    }

    [HttpGet]
    public IActionResult TransferToOwn()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult TransferToOwn(object model)
    {
        if (!ModelState.IsValid)
            return View(model);

        return RedirectToAction(nameof(Index), "Dashboard", new { area = "Customer" });
    }

    [HttpGet]
    public IActionResult TransferToThirdParty()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult TransferToThirdParty(object model)
    {
        if (!ModelState.IsValid)
            return View(model);

        return RedirectToAction(nameof(Index), "Dashboard", new { area = "Customer" });
    }
}