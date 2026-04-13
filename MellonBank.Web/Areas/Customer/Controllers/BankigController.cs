using Microsoft.AspNetCore.Mvc;

namespace MellonBank.Web.Areas.Customer.Controllers
{
    public class BankigController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyAccounts()
        {
            return View();
        }

        public IActionResult AccountDetails()
        {
            return View();
        }

        public IActionResult Balance()
        {
            return View();
        }

        public IActionResult TransferToOwn()
        {
            return View();
        }

        public IActionResult TransferToThirdParty()
        {
            return View();
        }
    }
}
