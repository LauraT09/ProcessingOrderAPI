namespace ProcessingOrderAPI.Models
{
    public class Orders
    {
        //type, orderno, description, value, qty
        public string Type { get; set; }
        public decimal OrderNumber { get; set; }

        public string Description { get; set; }

        public decimal Value { get; set; }
        public decimal Quantity { get; set; }
    }
}
