using Microsoft.AspNetCore.Mvc;
using ProcessingOrderAPI.Models;
using ProcessingOrderAPI.Services;

namespace ProcessingOrderAPI.Controllers
{

    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly ICSVService _csvService;
        private readonly IWebHostEnvironment _env;

        public OrderController(ICSVService csvService, IWebHostEnvironment env)
        {
            _csvService = csvService;
            _env = env;
        }

        [HttpGet("TopFiveOrders")]
        public IActionResult GetTopFiveOrders()
        {
            string filePath = Path.Combine(_env.ContentRootPath, "Data", "values.csv");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("CSV file not found");
            }

            var result = _csvService.GetTopFiveOrders(filePath);
            return Ok(result);
        }
    }
}

