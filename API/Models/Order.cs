using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    [PrimaryKey(nameof(Id))]
    public class Order
    {
        public int Id { get; set; }
        public Customer? Customer { get; set; }
        public int CustomerId { get; set; } // Foreign key to Customer
        public Product? Product { get; set; }
        public int ProductId { get; set; } // Foreign key to Product
        public int Quantity { get; set; }
        [Precision(11, 2)]
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } // e.g., "Pending", "Shipped", "Delivered"
        public DateTime OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; } // Nullable in case the order hasn't been shipped yet
        public DateTime? DeliveredDate { get; set; } // Nullable in case the order hasn't been delivered yet

    }
}
