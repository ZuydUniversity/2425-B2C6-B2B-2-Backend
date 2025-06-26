using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [PrimaryKey(nameof(Id))]
    public class Product
    {
        public int Id { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; } // e.g., "A", "B", "C"

        /// <summary>
        /// Price is the selling price of the product, which is the price customers pay.
        /// </summary>
        [Precision(11, 2)]
        public decimal Price { get; set; }

        /// <summary>
        /// CostPrice is the cost for the company to acquire the product.
        /// </summary>
        [Precision(11, 2)]
        public decimal CostPrice { get; set; } // Cost for the company to acquire the product

        /// <summary>
        /// Number of Blue blocks used to produce the product
        /// </summary>
        public int BlueBlocks { get; set; }
        /// <summary>
        /// Number of Red blocks used to produce the product
        /// </summary>
        public int RedBlocks { get; set; }
        /// <summary>
        /// Number of Grey blocks used to produce the product
        /// </summary>
        public int GreyBlocks { get; set; }

        /// <summary>
        /// ProductionTime is the time in seconds required to produce this product.
        /// </summary>
        public int ProductionTime { get; set; }
    }
}
