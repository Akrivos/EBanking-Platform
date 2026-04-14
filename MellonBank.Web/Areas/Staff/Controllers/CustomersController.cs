using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MellonBank.Web.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles = "Staff")]
    public class CustomersController : Controller
    {
        // OLOI OI PELATES
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllAsync();
            return View(customers);
        }

        // PLirofories pelati
        public IActionResult Details()
        {
            return View();
        }

        // Epeksergasia pliroforion pelati
        [HttpPut]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search(string afm)
        {
            return View();
        }
    }
}
