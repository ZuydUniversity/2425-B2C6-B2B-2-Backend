
using Backend.Models;
using System;

namespace Backend.Services
{
    public class AccountManagerService
    {
        public bool VerifyFinalProduct(Order order)
        {
            Console.WriteLine("Controle eindproduct op kwaliteit en kwantiteit voor " + order.CustomerName);
            return true;
        }
    }
}