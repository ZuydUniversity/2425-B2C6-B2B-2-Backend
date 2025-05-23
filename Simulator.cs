using System;

namespace Backend.Models
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

                bool approved = _accountManager.ApproveOrder(order);
                if (!approved)
                {
                    Console.WriteLine("Order wordt niet opgenomen in de planning.");
                }
                else
                {
                    Console.WriteLine("Order kan worden doorgezet naar planning.");
                }

                orderCount++;
            }

            Console.WriteLine("\nMaximaal aantal orders bereikt. Simulatie afgesloten.");
        }
    }
}
