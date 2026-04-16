using MellonBank.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MellonBank.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("GetRates")]
        public async Task<IActionResult> GetRates()
        {
            var rates = await _currencyService.GetRatesAsync();

            if (rates is null)
                return NotFound();

            return Ok(rates);
        }
    }
}
