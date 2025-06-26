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
            // 1. Maak een geldige order aan met alle vereiste velden
            var testOrder = new
            {
                CustomerId = 1,
                ProductId = 1,
                Quantity = 1,
                TotalPrice = 10000,
                Status = "Pending",
                OrderDate = DateTime.UtcNow,
                OrderType = "A",
                IsSignedByInkoop = true,
                IsSignedByAccountmanager = true,
                ForwardedToSupplier = false,
                PicklistStatus = "NotStarted",
                RejectionReason = (string?)null,
                Comment = "Test order"
            };

            var json = JsonConvert.SerializeObject(testOrder);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _output.WriteLine("[START] Order aanmaken via API");
            var response = await _httpClient.PostAsync("api/Orders", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"POST mislukt: {response.StatusCode} - {responseBody}");

            dynamic createdOrder = JsonConvert.DeserializeObject(responseBody);
            int createdOrderId = createdOrder.id;
            Assert.True(createdOrderId > 0, "Order ID ongeldig");
            _output.WriteLine($"[OK] Order aangemaakt met ID={createdOrderId}");

            // 2. Controleer of er een eventlog is vastgelegd
            var logResponse = await _httpClient.GetAsync("api/ProcessMining/log");
            Assert.True(logResponse.IsSuccessStatusCode, "Eventlog-opvraag mislukt");

            var logJson = await logResponse.Content.ReadAsStringAsync();
            var logEntries = JsonConvert.DeserializeObject<List<dynamic>>(logJson);

            var log = logEntries.FirstOrDefault(e => (int?)e.caseId == createdOrderId && ((string)e.activity).Contains("aangemaakt"));
            Assert.NotNull(log);
            _output.WriteLine($"[LOG OK] EventLog bevat aanmaakvermelding voor Order {createdOrderId}");

            // 3. Verwijder de order
            var deleteResponse = await _httpClient.DeleteAsync($"api/Orders/{createdOrderId}");
            if (!deleteResponse.IsSuccessStatusCode)
            {
                var error = await deleteResponse.Content.ReadAsStringAsync();
                throw new Exception($"DELETE mislukt: {deleteResponse.StatusCode} - {error}");
            }
            _output.WriteLine($"[CLEANUP] Order {createdOrderId} verwijderd via API");
        }
    }
}
