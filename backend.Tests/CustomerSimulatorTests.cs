using Backend.Models;
using Backend.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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

            // Configure real API
            sc.AddHttpClient<OrderService>(c =>
            {
                c.BaseAddress = new Uri("http://b2b2buildingblocks.westeurope.cloudapp.azure.com:8080/");
                c.Timeout = TimeSpan.FromMinutes(3); // verhoogde timeout
            });

            _services = sc.BuildServiceProvider();
        }

        [Fact]
        public async Task Full_Order_Creation_And_Logging_Flow_Works()
        {
            var orderService = _services.GetRequiredService<OrderService>();

            // 1. Create order
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

            Assert.NotNull(createdOrder);
            Assert.True(createdOrder.Id > 0);
            _output.WriteLine($"[OK] Order aangemaakt met ID={createdOrder.Id}");

            // 2. Verify eventlog (optioneel)
            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://b2b2buildingblocks.westeurope.cloudapp.azure.com:8080/")
            };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var logResponse = await httpClient.GetAsync("api/ProcessMining/log");
            Assert.True(logResponse.IsSuccessStatusCode, "Eventlog-opvraag mislukt");

            var logJson = await logResponse.Content.ReadAsStringAsync();
            var logEntries = JsonConvert.DeserializeObject<List<dynamic>>(logJson);

            var createdLog = logEntries.FirstOrDefault(e => (int)e.caseId == createdOrder.Id && ((string)e.activity).Contains("aangemaakt"));
            Assert.NotNull(createdLog);
            _output.WriteLine($"[LOG OK] EventLog bevat aanmaakvermelding voor Order {createdOrder.Id}");

            // 3. Delete order
            await orderService.DeleteAsync(createdOrder.Id);
            _output.WriteLine($"[CLEANUP] Order {createdOrder.Id} verwijderd");
        }
    }
}
