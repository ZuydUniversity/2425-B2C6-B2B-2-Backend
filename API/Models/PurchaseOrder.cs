namespace API.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public int ProductId { get; set; }
        public int SupplierId { get; set; }
        public int Quantity { get; set; }

    }
}
