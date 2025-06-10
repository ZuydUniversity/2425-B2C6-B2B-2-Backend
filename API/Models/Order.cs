using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    [PrimaryKey(nameof(Id))]
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Range(1, 3, ErrorMessage = "Aantal moet tussen 1 en 3 liggen.")]
        public int Quantity { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Delivered

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime? ApprovedDate { get; set; }
        public DateTime? RejectedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }

        public string? Comment { get; set; } // Optional message from account manager

        public Customer? Customer { get; set; }
        public Product? Product { get; set; }
    }
}
