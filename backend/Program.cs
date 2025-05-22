using System;
using System.Threading;
using backend.Models;
using backend.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace backend
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer("Server=sql,1433;Database=BuildingBlocksDb;User=sa;Password=Bl0ck$1234;TrustServerCertificate=True;"));


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

            Console.WriteLine("Simulatie afgerond. Container actief.");
            Thread.Sleep(Timeout.Infinite);
        }
    }
}

