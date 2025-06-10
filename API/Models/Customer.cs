using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Username), IsUnique = true)]
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        public List<Order>? Orders { get; set; }
    }
}