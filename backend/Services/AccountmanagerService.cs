
using backend.Models;
using System;

namespace backend.Services
{
    public class AccountmanagerService
    {
        public bool ControleerEindproduct(Order order)
        {
            Console.WriteLine("Controle eindproduct op kwaliteit en kwantiteit voor " + order.KlantNaam);
            return true;
        }
    }
}
