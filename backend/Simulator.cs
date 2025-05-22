using System;

namespace backend.Models
{
    public class Simulator
    {
        private readonly Random _random = new();

        public void StartInteractieveSimulatie()
        {
            var klant = new Customer(1);
            Console.WriteLine("Welkom bij de order simulator!");

            while (true)
            {
                Console.Write("\nOrder plaatsen? (druk op Enter, of typ 'q' om te stoppen): ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (input == "q")
                {
                    Console.WriteLine("Simulatie gestopt.");
                    break;
                }

                var order = klant.PlaatsOrder(_random);

                Console.WriteLine($"✅ {order.KlantNaam} plaatst een order: {order.Type}, aantal: {order.Aantal}");
            }
        }
    }
}
