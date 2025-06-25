namespace API.Models
{
    public class PartsDelivery
    {
        public int Id { get; set; }
        public string PartsReference { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsComplete { get; set; }
    }
}
