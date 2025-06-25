namespace API.Models
{
    public class Expedition
    {
        public int Id { get; set; }
        public string ShipmentReference { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string Destination { get; set; }
        public bool IsDelivered { get; set; }
    }
}
