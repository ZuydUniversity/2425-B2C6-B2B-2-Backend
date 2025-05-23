using System;

namespace backend.Models
{
    public class AccountManager
    {
        public bool ApproveOrder(Order order)
        {
            if (!order.IsValid)
            {
                Console.WriteLine($"Order van {order.CustomerName} is ongeldig (aantal {order.Quantity})");
                return false;
            }

            if (string.IsNullOrWhiteSpace(order.CustomerName) || order.OrderDate == default)
            {
                Console.WriteLine("Order mist klantnaam of besteldatum");
                return false;
            }

            order.IsApproved = true;
            order.Status = OrderStatus.Approved;
            Console.WriteLine($"Order van {order.CustomerName} is geaccordeerd.");
            return true;
        }
    }
}


