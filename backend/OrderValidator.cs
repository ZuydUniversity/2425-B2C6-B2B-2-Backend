namespace backend
{
    public class Order
    {
        public string ProductType { get; set; } = "";
        public int Quantity { get; set; }
        public bool HasSignature { get; set; }
        public DateTime? OrderDate { get; set; }
    }

    public class OrderValidator
    {
        public bool IsValid(Order order)
        {
            var validTypes = new[] { "A", "B", "C" };
            if (!validTypes.Contains(order.ProductType)) return false;
            if (order.Quantity < 1 || order.Quantity > 3) return false;
            if (!order.HasSignature) return false;
            if (order.OrderDate == null) return false;

            return true;
        }
    }
}

