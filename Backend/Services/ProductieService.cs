
using Backend.Models;
using System;

namespace Backend.Services
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

