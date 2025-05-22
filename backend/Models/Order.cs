using System;

namespace backend.Models
{
    public class Order
    {
        public string CustomerName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsValid => Quantity >= 1 && Quantity <= 3;


    }
    public enum OrderStatus
    {
        New,
        Approved,
        Rejected,
        Scheduled,
        InProduction,
        Ready,
        ReadyToShip,
        Shipped,
        Invoiced
    }


}
