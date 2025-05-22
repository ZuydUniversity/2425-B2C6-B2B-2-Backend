using System;

namespace backend.Models
{
    public class Simulator
    {
        private readonly Random _random = new();

        public void StartInteractieveSimulatie()
        {
            var klant = new Customer(1);
            var accountManager = new AccountManager();
            int aantalOrders = 0;
            int maxOrders = 3;

            Console.WriteLine("Welkom bij de BuildingBlocks simulatie");

            while (aantalOrders < maxOrders)
            {
                Console.Write($"\nOrder plaatsen ({aantalOrders + 1}/{maxOrders})? (Enter = Ja / q = Stoppen): ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (input == "q")
                {
                    Console.WriteLine("Simulatie handmatig gestopt.");
                    return;
                }

                var order = klant.PlaatsOrder(_random);
                Console.WriteLine($"Order aangemaakt: {order.Type}, aantal: {order.Aantal}");

                bool akkoord = accountManager.AccordeerOrder(order);
                if (!akkoord)
                {
                    Console.WriteLine("Order wordt niet opgenomen in de planning.");
                }
                else
                {
                    Console.WriteLine("Order kan worden doorgezet naar planning.");
                }

                aantalOrders++;
            }

            Console.WriteLine("\nMaximaal aantal orders bereikt. Simulatie afgesloten.");
        }
    }
}
