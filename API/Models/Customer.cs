using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Username), IsUnique = true)]
    public class Customer
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}