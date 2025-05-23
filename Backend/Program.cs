using Backend;
using Backend.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
               
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=sqlserver;Database=BuildingBlocksDb;User Id=sa;Password=Bl0ck$1234;TrustServerCertificate=True;");



        services.AddTransient<Simulator>();
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); 
}

var simulator = host.Services.GetRequiredService<Simulator>();
simulator.StartInteractiveSimulation();

