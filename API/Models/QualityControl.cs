namespace API.Models
{
    public class QualityControl
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
        public Product Product { get; set; }
    }
}
