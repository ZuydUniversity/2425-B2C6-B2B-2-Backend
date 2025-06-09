using Backend.Models;
using Backend.Services;

namespace Backend.Simulator
{
    public class CustomerSimulator
    {
        private readonly CustomerService _customerService;
        private readonly OrderService _orderService;

        private readonly List<Order> _openOrders = new();
        private readonly Random _random = new();

        public CustomerSimulator(CustomerService customerService, OrderService orderService)
        {
            _customerService = customerService;
            _orderService = orderService;
        }

        public async Task StartAsync()
        {
            // 1. Bepaal type (A, B of C)
            string[] types = { "A", "B", "C" };
            string type = types[_random.Next(0, 3)];

            // 2. Bepaal aantal (1 of 2)
            int quantity = _random.Next(1, 3);

            // 3. Voeg toe aan open orders
            var order = new Order
            {
                CustomerId = 1,
                ProductId = type switch { "A" => 1, "B" => 2, "C" => 3, _ => 1 },
                Quantity = quantity,
                TotalPrice = 100 * quantity,
                Status = "Pending",
                OrderDate = DateTime.UtcNow
            };

            _openOrders.Add(order);

            // 4. Stuur order via API
            await _orderService.CreateAsync(order);

            Console.WriteLine($"Order aangemaakt: {type} x{quantity}");

            // 5. Simuleer levering
            await ProcessDeliveryAsync(order);
        }

        private async Task ProcessDeliveryAsync(Order order)
        {
            // 6. Controleer order (simuleer met kans op fout)
            bool goedgekeurd = _random.Next(0, 100) < 80; // 80% kans op goed

            if (goedgekeurd)
            {
                Console.WriteLine("✅ Order goedgekeurd.");
                _openOrders.Remove(order);
                // berg artikel op (optioneel registreren)
            }
            else
            {
                Console.WriteLine("❌ Order afgekeurd — artikel in afvaldoos.");
                // optioneel: registreer afkeuring
            }
        }
    }
}
