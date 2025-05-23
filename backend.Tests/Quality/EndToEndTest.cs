using Xunit;
using Backend.Models;
using Backend.Services;

namespace Backend.Tests.Quality
{
    public class EndToEndTest
    {
        [Fact]
        public void FullFlow_ShouldExecuteSuccessfully()
        {
            var order = new Order
            {
                CustomerName = "FlowTest",
                Type = MotorType.C,
                Quantity = 2
            };

            var orderService = new OrderService();
            var production = new ProductionService();
            var account = new AccountManagerService();
            var shipping = new ShippingService();
            var invoicing = new InvoicingService();

            Assert.True(orderService.ValidateAndApprove(order));
            Assert.True(production.CheckMaterialsAndStart(order));
            Assert.True(account.VerifyFinalProduct(order));

            shipping.DispatchOrder(order);
            invoicing.GenerateInvoice(order);
        }
    }
}