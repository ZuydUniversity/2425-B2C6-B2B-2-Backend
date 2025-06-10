namespace Backend.Models
{
    public class ProductionLine
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<WorkOrder> WorkOrders { get; set; } = new();
    }
}
