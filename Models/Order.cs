using System;

namespace backend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public MotorType Type { get; set; }
        public int Quantity { get; set; }
        public bool IsApproved { get; set; } = false;
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.New;

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
