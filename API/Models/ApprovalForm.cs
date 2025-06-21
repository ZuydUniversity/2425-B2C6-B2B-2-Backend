namespace API.Models
{
    public class ApprovalForm
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }
    }
}
