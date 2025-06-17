using ProcessingOrderAPI.Models;

namespace ProcessingOrderAPI.Services
{
    public interface ICSVService
    {
        List<OrderSummary> GetTopFiveOrders(string csvPath); 
    }
}
