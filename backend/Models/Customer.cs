using System;

namespace backend.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Naam => $"Klant #{Id}";

        public Customer(int id)
        {
            Id = id;
        }

        public Order PlaatsOrder(Random random)
        {
            return new Order
            {
                KlantNaam = Naam,
                Type = (MotorType)random.Next(0, 3),
                Aantal = random.Next(1, 4),
                Besteldatum = DateTime.Now
            };
        }
    }
}
