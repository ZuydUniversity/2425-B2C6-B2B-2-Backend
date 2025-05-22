using System;
using backend.Models;
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
                    services.AddTransient<Simulator>();
                })
                .Build();

            var simulator = host.Services.GetRequiredService<Simulator>();
            simulator.StartInteractiveSimulation();
        }
    }
}

