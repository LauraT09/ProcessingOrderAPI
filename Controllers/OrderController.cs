using Microsoft.AspNetCore.Mvc;
using ProcessingOrderAPI.Models;
using ProcessingOrderAPI.Services;

namespace ProcessingOrderAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly ICSVService _csvService;

        public OrderController(ICSVService csvService)
        {
            _csvService = csvService;
        }
       
        [HttpPost("read-orders-csv")]
        public async Task<IActionResult> GetOrdersCSV([FromForm] IFormFileCollection file)
        {
            var orders = _csvService.ReadCSV<Orders>(file[0].OpenReadStream());

            return Ok(orders);
        }
    }
}

