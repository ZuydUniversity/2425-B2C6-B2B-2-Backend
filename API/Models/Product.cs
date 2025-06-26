using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [PrimaryKey(nameof(Id))]
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } // e.g., "A", "B", "C"

        [Precision(11, 2)]
        public decimal Price { get; set; }

        [Precision(11, 2)]
        public decimal CostPrice { get; set; } // Cost for the company to acquire the product

        public int BlueBlocks { get; set; } // Number of blue blocks in the product
        public int RedBlocks { get; set; } // Number of red blocks in the product
        public int GreyBlocks { get; set; } // Number of green blocks in the product
    }
}
