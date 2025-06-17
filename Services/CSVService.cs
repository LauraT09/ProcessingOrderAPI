
using ProcessingOrderAPI.Models;
using System.Globalization;

namespace ProcessingOrderAPI.Services
{
    public class CSVService : ICSVService
    {
        private const string ProductType = "PRD";
        private const string CreditType = "CR";
        private const int MinimumPRDLength = 5;
        private const int MinimumCRLength = 4;
        private const int TopNumber = 5;
        public List<OrderSummary> GetTopFiveOrders(string csvPath)
        {
            var orderTotals = new Dictionary<int, decimal>();
            var lines = File.ReadLines(csvPath).Skip(1);

            foreach (var line in lines)
            {
                ProcessLine(line, orderTotals);
            }

            return GetTopOrders(orderTotals, TopNumber);
        }

        private void ProcessLine(string line, Dictionary<int, decimal> orderTotals)
        {
            var fields = line.Split(',', StringSplitOptions.TrimEntries);
            if (fields.Length < 3) return;

            string type = fields[0];
            if (!int.TryParse(fields[1], out int orderNo)) return;

            if (type == ProductType)
                ProcessProductLine(fields, orderNo, orderTotals);
            if (type == CreditType)
                ProcessCreditLine(fields, orderNo, orderTotals);
        }

        private void ProcessProductLine(string[] fields, int orderNo, Dictionary<int, decimal> orderTotals)
        {
            if (fields.Length < MinimumPRDLength) return;

            if (decimal.TryParse(fields[3], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price) &&
                int.TryParse(fields[4], out int quantity))
            {
                AddToOrderTotal(orderTotals, orderNo, price * quantity);
            }
        }

        private void ProcessCreditLine(string[] fields, int orderNo, Dictionary<int, decimal> orderTotals)
        {
            if (fields.Length < MinimumCRLength) return;

            if (decimal.TryParse(fields[3], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal credit))
            {
                AddToOrderTotal(orderTotals, orderNo, -credit);
            }
        }

        private void AddToOrderTotal(Dictionary<int, decimal> totals, int orderNo, decimal value)
        {
            if (!totals.ContainsKey(orderNo))
                totals[orderNo] = 0;

            totals[orderNo] += value;
        }

        private List<OrderSummary> GetTopOrders(Dictionary<int, decimal> orderTotals, int topN)
        {
            return orderTotals
                .OrderByDescending(order => order.Value)
                .Take(topN)
                .Select((order, index) => new OrderSummary
                {
                    Rank = index + 1,
                    OrderNo = order.Key,
                    Total = Math.Round(order.Value, 2)
                })
                .ToList();
        }
    }
}
