using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [PrimaryKey(nameof(Id))]
    public class PurchaseOrder
    {
        /// <summary>
        /// Een bestelling, wanneer deze is aangemaakt veranderd de status van de order in 'Waiting on Materials'.
        /// Dit bevat het ordernummer en het product
        /// </summary>

        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeliverDate { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; } // Pending, Delivered, Cancelled
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public int ProductionlineID { get; set; }
        public ProductionLine? ProductLine { get; set; }
    }
}
