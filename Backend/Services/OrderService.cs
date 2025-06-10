using Backend.Models;

namespace Backend.Services
{
    public class OrderService
    {
        private readonly HttpClient _http;

        public OrderService(HttpClient http)
        {
            _http = http;
        }

        public async Task CreateAsync(Order order)
        {
            var response = await _http.PostAsJsonAsync("api/Orders", order);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Order> CreateAndReturnAsync(Order order)
        {
            var response = await _http.PostAsJsonAsync("api/Orders", order);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Order>();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/Orders/{id}");
            response.EnsureSuccessStatusCode();
        }



    }
}
