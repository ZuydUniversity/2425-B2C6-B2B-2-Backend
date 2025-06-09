
    using System;

    namespace Backend.Models
    {
        public class Order
        {
            public int Id { get; set; }
            public int CustomerId { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }
            public string Status { get; set; } = "Pending";
            public DateTime OrderDate { get; set; }
            public DateTime? ShippedDate { get; set; }
            public DateTime? DeliveredDate { get; set; }
        }
    }


