using Backend.Models;
using Microsoft.EntityFrameworkCore;


namespace Data
{
    public class SQLServerDatabaseContext : DbContext
    {
        public SQLServerDatabaseContext(DbContextOptions<SQLServerDatabaseContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        
    }
}
