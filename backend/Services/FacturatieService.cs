
using backend.Models;
using System;

namespace backend.Services
{
    public class FacturatieService
    {
        public void Factureer(Order order)
        {
            Console.WriteLine($"Factuur aangemaakt voor {order.KlantNaam} voor {order.Aantal} motor(en).");
        }
    }
}
