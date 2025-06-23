namespace API.Models
{
    public class Picklist
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public string Type { get; set; } // A, B, or C
        public string Components { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
