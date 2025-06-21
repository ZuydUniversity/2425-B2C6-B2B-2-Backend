namespace API.Models
{
    public class RejectionForm
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public string Reason { get; set; }
        public DateTime RejectionDate { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }
    }
}
