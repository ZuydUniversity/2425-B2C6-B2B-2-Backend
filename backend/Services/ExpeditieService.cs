using backend.Models;
using System;

namespace backend.Services
{
    public class ShippingService
    {
        public void DispatchOrder(Order order)
        {
            Console.WriteLine($"Verzending gestart voor order van {order.CustomerName}");
        }
    }
}

