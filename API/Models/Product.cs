using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    [PrimaryKey(nameof(Id))]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., "A", "B", "C"
        public string Description { get; set; }
        [Precision(11, 2)]
        public decimal Price { get; set; }
        [Precision(11,2)]
        public decimal CostPrice { get; set; } // Cost for the company to acquire the product
        public int StockQuantity { get; set; }
    }
}
