using System;

namespace backend.Models
{
    public class AccountManager
    {
        public bool AccordeerOrder(Order order)
        {
            if (!order.IsGeldig)
            {
                Console.WriteLine($" Order van {order.KlantNaam} is ongeldig (aantal {order.Aantal})");
                return false;
            }

            // Optioneel: check of de klantnaam en datum ingevuld zijn
            if (string.IsNullOrWhiteSpace(order.KlantNaam) || order.Besteldatum == default)
            {
                Console.WriteLine($" Order mist klantnaam of besteldatum");
                return false;
            }

            order.Geaccordeerd = true;
            order.Status = OrderStatus.Goedgekeurd;
            Console.WriteLine($" Order van {order.KlantNaam} is geaccordeerd.");
            return true;
        }
    }
}
