using API.Helpers;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Data
{
    public class SQLServerDatabaseContext : DbContext
    {
        private readonly IOptions<AppSettings> _appSettings;

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }
        public DbSet<ProductionLine> ProductionLines { get; set; }
        public DbSet<Planning> Planning { get; set; }



        public SQLServerDatabaseContext(DbContextOptions options, IOptions<AppSettings> appSettings) : base(options)
        {
            _appSettings = appSettings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_appSettings.Value.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventLog>()
                .HasOne(e => e.Order)
                .WithMany(o => o.EventLogs)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
