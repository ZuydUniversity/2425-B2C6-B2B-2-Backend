namespace API.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public string DeliveryReference { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}
