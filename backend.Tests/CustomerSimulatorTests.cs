using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace backend.Tests
{

    public class CustomerSimulatorTests
    {
        private readonly HttpClient _httpClient;
        private readonly ITestOutputHelper _output;

        public CustomerSimulatorTests(ITestOutputHelper output)
        {
            _output = output;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://b2b2buildingblocks.westeurope.cloudapp.azure.com:8080/")
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Fact]
        public async Task Full_Order_Creation_And_Logging_Flow_Works()
        {
            // 1. Maak customer aan
            var newCustomer = new
            {
                Username = "testuser_" + Guid.NewGuid(),
                Name = "Test Gebruiker",
                Password = "Test123!"
            };
            var customerContent = new StringContent(JsonConvert.SerializeObject(newCustomer), Encoding.UTF8, "application/json");
            var customerResp = await _httpClient.PostAsync("api/Customers", customerContent);
            Assert.True(customerResp.IsSuccessStatusCode, $"Customer POST mislukt: {customerResp.StatusCode}");
            var customerObj = JsonConvert.DeserializeObject<dynamic>(await customerResp.Content.ReadAsStringAsync());
            int customerId = customerObj.id;

            // 2. Maak product aan
            var newProduct = new
            {
                Name = "TestProduct_" + Guid.NewGuid(),
                Description = "Testproduct voor integratietest",
                Price = 999.99,
                CostPrice = 500.00,
                StockQuantity = 10
            };
            var productContent = new StringContent(JsonConvert.SerializeObject(newProduct), Encoding.UTF8, "application/json");
            var productResp = await _httpClient.PostAsync("api/Products", productContent);
            Assert.True(productResp.IsSuccessStatusCode, $"Product POST mislukt: {productResp.StatusCode}");
            var productObj = JsonConvert.DeserializeObject<dynamic>(await productResp.Content.ReadAsStringAsync());
            int productId = productObj.id;

            // 3. Maak order aan
            var newOrder = new
            {
                CustomerId = customerId,
                ProductId = productId,
                Quantity = 1,
                TotalPrice = 999.99,
                Status = "Pending",
                OrderDate = DateTime.UtcNow
            };
            var orderContent = new StringContent(JsonConvert.SerializeObject(newOrder), Encoding.UTF8, "application/json");
            var orderResp = await _httpClient.PostAsync("api/Orders", orderContent);
            Assert.True(orderResp.IsSuccessStatusCode, $"Order POST mislukt: {orderResp.StatusCode}");
            var orderObj = JsonConvert.DeserializeObject<dynamic>(await orderResp.Content.ReadAsStringAsync());
            int orderId = orderObj.id;

            // 4. Check eventlog
            var logResp = await _httpClient.GetAsync("api/ProcessMining/log");
            Assert.True(logResp.IsSuccessStatusCode, "Eventlog ophalen mislukt");
            var logs = JsonConvert.DeserializeObject<List<dynamic>>(await logResp.Content.ReadAsStringAsync());
            var log = logs.FirstOrDefault(e => (int)e.caseId == orderId && ((string)e.activity).Contains("aangemaakt"));
            Assert.NotNull(log);

            // 5. Opschonen (verwijder volgorde: order → product → klant)
            await _httpClient.DeleteAsync($"api/Orders/{orderId}");
            await _httpClient.DeleteAsync($"api/Products/{productId}");
            await _httpClient.DeleteAsync($"api/Customers/{customerId}");
        }

    }


}

