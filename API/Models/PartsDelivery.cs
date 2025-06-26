namespace API.Models
{
    public class PartsDelivery
    {
        public int PartsDeliveryId { get; set; }
        public string PartsReference { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsComplete { get; set; }

        public Order Order { get; set; } // Relatie met Order
    }
}
