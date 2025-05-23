using System;
using Backend.Models;

namespace Backend
{
    public class Simulator
    {
        private readonly Random _random = new();
        private readonly Customer _customer;
        private readonly AccountManager _accountManager;

        public Simulator()
        {
            _customer = new Customer(1);
            _accountManager = new AccountManager();
        }

        public void StartInteractiveSimulation()
        {
            int orderCount = 0;
            int maxOrders = 3;

            Console.WriteLine("Welkom bij de BuildingBlocks simulatie");

            while (orderCount < maxOrders)
            {
                Console.Write($"\nOrder plaatsen ({orderCount + 1}/{maxOrders})? (Enter = Ja / q = Stoppen): ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (input == "q")
                {
                    Console.WriteLine("Simulatie handmatig gestopt.");
                    return;
                }

                var order = _customer.PlaceOrder(_random);
                Console.WriteLine($"Order aangemaakt: {order.Type}, aantal: {order.Quantity}");

                try
                {
                    _accountManager.CheckOrderCompleteness(order);

                    bool approved = _accountManager.ApproveOrder(order);
                    if (!approved)
                    {
                        Console.WriteLine("Order wordt niet opgenomen in de planning.");
                        continue;
                    }

                    _accountManager.SignOrder(order);
                    _accountManager.AddToWorkInProgress(order);

                    // Simuleer kwaliteitsprobleem willekeurig
                    order.HasQualityIssues = _random.NextDouble() < 0.2;

                    _accountManager.CheckProductQuality(order);
                    _accountManager.CheckOffWorkInProgress(order);
                    _accountManager.DeliverOrder(order);

                    Console.WriteLine("Order succesvol verwerkt en geleverd.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fout tijdens orderverwerking: {ex.Message}");
                }

                orderCount++;
            }

            Console.WriteLine("\nMaximaal aantal orders bereikt. Simulatie afgesloten.");
        }
    }
}
