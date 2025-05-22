using System;

namespace backend.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Customer(int id)
        {
            Id = id;
        }

        public Order PlaceOrder(Random random)
        {
            return new Order
            {
                CustomerName = Name,
                Type = (MotorType)random.Next(0, 3),
                Quantity = random.Next(1, 4),
                OrderDate = DateTime.Now
            };
        }
    }
}

