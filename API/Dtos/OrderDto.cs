namespace API.Dtos
{
    public class OrderDto
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string PicklistStatus { get; set; }
        public string RejectionReason { get; set; }
    }
}
