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

            modelBuilder.Entity<EventLog>()
                .HasOne(e => e.Order)
                .WithMany(o => o.EventLogs)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            Product ProductA = new()
            {
                Id = 1,
                Name = "A",
                Price = 30000.00m,
                CostPrice = 30000.00m,
                BlueBlocks = 4,
                RedBlocks = 4,
                GreyBlocks = 2,
                ProductionTime = 20
            };

            Product ProductB = new()
            {
                Id = 2,
                Name = "B",
                Price = 24000.00m,
                CostPrice = 24000.00m,
                BlueBlocks = 2,
                RedBlocks = 2,
                GreyBlocks = 4,
                ProductionTime = 10
            };

            Product ProductC = new()
            {
                Id = 3,
                Name = "C",
                Price = 21000.00m,
                CostPrice = 21000.00m,
                BlueBlocks = 3,
                RedBlocks = 2,
                GreyBlocks = 2,
                ProductionTime = 15
            };

            modelBuilder.Entity<Product>().HasData(ProductA, ProductB, ProductC);
            modelBuilder.Entity<ProductionLine>().HasData(
                new ProductionLine { Id = 1, Name = "Lijn 1", IsActive = true },
                new ProductionLine { Id = 2, Name = "Lijn 2", IsActive = true },
                new ProductionLine { Id = 3, Name = "Lijn 3", IsActive = true}
            );
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Customer 1" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
