using Backend.Models;
using System;

namespace Backend.Services
{
    public class InvoicingService
    {
        public void GenerateInvoice(Order order)
        {
            Console.WriteLine($"Factuur aangemaakt voor {order.CustomerName} voor {order.Quantity} motor(en).");
        }
    }
}

