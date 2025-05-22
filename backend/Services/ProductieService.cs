
using backend.Models;
using System;

namespace backend.Services
{
    public class ProductionService
    {
        public bool CheckMaterialsAndStart(Order order)
        {
            Console.WriteLine("Controle materialen voor order van " + order.CustomerName);
            return order.IsApproved;
        }
    }
}

