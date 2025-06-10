using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., "A", "B", "C"
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; } // Cost for the company to acquire the product
        public int StockQuantity { get; set; }
    }
}
