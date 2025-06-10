
namespace Backend.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Username { get; set; }


        public string Name { get; set; }

        public string Password { get; set; }

        public List<Order>? Orders { get; set; }
    }
}
