using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MellonBank.Web.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles = "Staff")]
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateCustomer()
        {
            return View();
        }

        public IActionResult CreateStaff()
        {
            return View();
        }
    }
}
