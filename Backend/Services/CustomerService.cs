using Backend.Models;

namespace Backend.Services
{
    public class CustomerService
    {
        private readonly HttpClient _http;

        public CustomerService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            var customers = await _http.GetFromJsonAsync<List<Customer>>("api/Customers");
            return customers ?? new List<Customer>();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<Customer>($"api/Customers/{id}");
        }

        public async Task CreateAsync(Customer customer)
        {
            await _http.PostAsJsonAsync("api/Customers", customer);
        }

        public async Task UpdateAsync(Customer customer)
        {
            await _http.PutAsJsonAsync($"api/Customers/{customer.Id}", customer);
        }
    }
}
