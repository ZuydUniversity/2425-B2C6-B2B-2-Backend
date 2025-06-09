using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace backend.Tests
{
    public class CustomerSimulatorTests
    {
        private readonly ServiceProvider _services;
        private readonly ITestOutputHelper _output;

        public CustomerSimulatorTests(ITestOutputHelper output)
        {
            _output = output;

            var sc = new ServiceCollection();

            sc.AddHttpClient<OrderService>(c =>
            {
                c.BaseAddress = new Uri("http://b2b2buildingblocks.westeurope.cloudapp.azure.com:8080/");
            });

            _services = sc.BuildServiceProvider();
        }

        [Fact]
        public async Task CreateAndDeleteOrder_WorksCorrectly()
        {
            var orderService = _services.GetRequiredService<OrderService>();

            var testOrder = new Order
            {
                CustomerId = 1,
                ProductId = 1,
                Quantity = 1,
                TotalPrice = 999,
                Status = "Pending",
                OrderDate = DateTime.UtcNow
            };

            _output.WriteLine($"[START] Order aanmaken voor CustomerId={testOrder.CustomerId}");

            var createdOrder = await orderService.CreateAndReturnAsync(testOrder);

            _output.WriteLine($"[INFO] Aangemaakt met ID={createdOrder.Id}, status={createdOrder.Status}");

            Assert.NotNull(createdOrder);
            Assert.True(createdOrder.Id > 0);

            await orderService.DeleteAsync(createdOrder.Id);

            _output.WriteLine($"[CLEANUP] Verwijderd ID={createdOrder.Id}");
        }
    }
}

