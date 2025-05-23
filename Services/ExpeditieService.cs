
using Backend.Models;
using System;

namespace Backend.Services
{
    public class ShippingService
    {
        public void DispatchOrder(Order order)
        {
            Console.WriteLine($"Verzending gestart voor order van {order.CustomerName}");
        }
    }
}