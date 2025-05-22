
using backend.Models;
using System;

namespace backend.Services
{
    public class ProductieService
    {
        public bool ControleerMaterialenEnStart(Order order)
        {
            Console.WriteLine("Controle materialen voor order van " + order.KlantNaam);
            return order.Geaccordeerd;
        }
    }
}
