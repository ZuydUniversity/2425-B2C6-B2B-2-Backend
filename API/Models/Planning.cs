namespace API.Models
{
    public class Planning
    {
        public int Id { get; set; }
        public DateTime PlannedDate { get; set; } = DateTime.UtcNow;
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductionLineId { get; set; }
        public ProductionLine? ProductionLine { get; set; }
    }
}
