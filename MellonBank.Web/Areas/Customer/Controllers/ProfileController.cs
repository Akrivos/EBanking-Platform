using Microsoft.AspNetCore.Mvc;

namespace MellonBank.Web.Areas.Customer.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult MyProfile()
        {
            return View();
        }
    }
}
