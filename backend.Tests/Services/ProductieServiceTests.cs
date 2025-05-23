using Xunit;
using Backend.Models;
using Backend.Services;

namespace Backend.Tests.Services
{
    public class ProductionServiceTests
    {
        [Fact]
        public void StartProduction_OnlyIfOrderApproved()
        {
            var order = new Order { CustomerName = "Test", Quantity = 1, IsApproved = true };
            var production = new ProductionService();

            var result = production.CheckMaterialsAndStart(order);

            Assert.True(result);
        }

        [Fact]
        public void StartProduction_FailsWithoutApproval()
        {
            var order = new Order { CustomerName = "Test", Quantity = 1, IsApproved = false };
            var production = new ProductionService();

            var result = production.CheckMaterialsAndStart(order);

            Assert.False(result);
        }
    }
}
