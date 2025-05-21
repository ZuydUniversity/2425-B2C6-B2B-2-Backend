
using Xunit;
using backend.Models;
using backend.Services;

namespace backend.Tests.Services
{
    public class OrderServiceTests
    {
        [Fact]
        public void ValideOrder_WordtGeaccordeerd()
        {
            var order = new Order
            {
                KlantNaam = "TestKlant",
                Type = MotorType.B,
                Aantal = 2
            };

            var service = new OrderService();
            var result = service.ValideerEnAccordeer(order);

            Assert.True(result);
            Assert.True(order.Geaccordeerd);
        }

        [Fact]
        public void OngeldigeOrder_WordtNietGeaccordeerd()
        {
            var order = new Order
            {
                KlantNaam = "TestKlant",
                Type = MotorType.A,
                Aantal = 5
            };

            var service = new OrderService();
            var result = service.ValideerEnAccordeer(order);

            Assert.False(result);
            Assert.False(order.Geaccordeerd);
        }
    }
}
