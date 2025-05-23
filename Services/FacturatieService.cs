using backend.Models;
using System;

namespace backend.Services
{
    public class InvoicingService
    {
        public void GenerateInvoice(Order order)
        {
            Console.WriteLine($"Factuur aangemaakt voor {order.CustomerName} voor {order.Quantity} motor(en).");
        }
    }
}

