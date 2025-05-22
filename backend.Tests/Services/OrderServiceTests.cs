using Xunit;
using backend.Models;
using backend.Services;

namespace backend.Tests.Services
{
    public class OrderServiceTests
    {
        [Fact]
        public void ValidOrder_ShouldBeApproved()
        {
            var order = new Order
            {
                CustomerName = "TestCustomer",
                Type = MotorType.B,
                Quantity = 2
            };

            var service = new OrderService();
            var result = service.ValidateAndApprove(order);

            Assert.True(result);
            Assert.True(order.IsApproved);
        }

        [Fact]
        public void InvalidOrder_ShouldNotBeApproved()
        {
            var order = new Order
            {
                CustomerName = "TestCustomer",
                Type = MotorType.A,
                Quantity = 5
            };

            var service = new OrderService();
            var result = service.ValidateAndApprove(order);

            Assert.False(result);
            Assert.False(order.IsApproved);
        }
    }
}
