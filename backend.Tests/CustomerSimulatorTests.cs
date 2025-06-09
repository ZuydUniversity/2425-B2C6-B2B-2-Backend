using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace backend.Tests
{
    public class CustomerSimulatorTests
    {
        private readonly ServiceProvider _services;

        public CustomerSimulatorTests()
        {
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

            var createdOrder = await orderService.CreateAndReturnAsync(testOrder);

            Assert.NotNull(createdOrder);
            Assert.True(createdOrder.Id > 0);

            await orderService.DeleteAsync(createdOrder.Id);
        }
    }
}
