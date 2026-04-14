using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MellonBank.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class DashboardController : Controller
    {
        // OLOI OI LOGARIASMOS TOU PELATI - epilogi logariasmou
        public IActionResult Index()
        {
            return View();
        }
    }
}
