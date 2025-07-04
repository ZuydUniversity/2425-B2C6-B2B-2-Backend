using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models
{
    [PrimaryKey(nameof(Id))]
    public class Order
    {
        public int Id { get; set; }

        /// <summary>
        /// CustomerId is the ID of the customer who placed the order.
        /// </summary>
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// ProductId is the ID of the product being ordered.
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Quantity is the number of products ordered by the customer.
        /// </summary>
        [Range(1, 3, ErrorMessage = "Aantal moet tussen 1 en 3 liggen.")]
        public int Quantity { get; set; }

        /// <summary>
        /// TotalPrice is the total price of the order, calculated as Quantity * Product.Price.
        /// </summary>
        public decimal? TotalPrice { get; set; }

        /// <summary>
        /// Status indicates the current status of the order. e.g. Pending, Approved, Rejected, Delivered, Planned, Waiting on Materials
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// OrderDate is the date and time when the order was placed.
        /// </summary>
        [Required]
        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ApprovedDate is the date and time when the order was approved by the account manager (if necessary).
        /// </summary>
        public DateTime? ApprovedDate { get; set; }

        /// <summary>
        /// RejectedDate is the date and time when the order was rejected by the account manager.
        /// </summary>
        public DateTime? RejectedDate { get; set; }

        /// <summary>
        /// DeliveredDate is the date and time when the order was delivered to the customer.
        /// </summary>
        public DateTime? DeliveredDate { get; set; }

        /// <summary>
        /// Comment is an optional message from the account manager regarding the order.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Approved indicates whether the order has been approved by the account manager.
        /// </summary>
        public bool Approved { get; set; } = false;

        /// <summary>
        /// ForwardedToSupplier indicates whether the order has been forwarded to the supplier for parts.
        /// </summary>
        public bool ForwardedToSupplier { get; set; } = false;

        /// <summary>
        /// RejectionReason is an optional reason for rejecting the order, provided by the account manager.
        /// </summary>
        public string? RejectionReason { get; set; }

        // Computed property (niet in database opgeslagen)
        /// <summary>
        /// RequiresApproval indicates whether the order requires approval based on the total price.
        /// </summary>
        [JsonIgnore]
        public bool RequiresApproval => TotalPrice > 50000;

        public Customer? Customer { get; set; }
        public Product? Product { get; set; }
        public List<EventLog> EventLogs { get; set; } = new();
    }
}
