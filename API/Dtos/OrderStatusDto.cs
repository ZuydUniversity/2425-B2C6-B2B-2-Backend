namespace API.Dtos
{
    public class OrderStatusDto
    {
        public string Status { get; set; } = "Pending"; // Approved, Rejected, Delivered
        public string? Comment { get; set; } // Commentaar toevoegen voor terugkoppeling aan de klant.
        public int CustomerId { get; set; }
    }
}

