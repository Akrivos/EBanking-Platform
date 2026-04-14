using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MellonBank.Web.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize(Roles = "Customer")]
public class ProfileController : Controller
{
    [HttpGet]
    public IActionResult MyProfile()
    {
        return View();
    }
}