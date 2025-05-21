
using Xunit;
using backend.Models;
using backend.Services;

namespace backend.Tests.Quality
{
    public class EndToEndTest
    {
        [Fact]
        public void VolledigeFlow_KanUitgevoerdWorden()
        {
            var order = new Order
            {
                KlantNaam = "FlowTest",
                Type = MotorType.C,
                Aantal = 2
            };

            var orderService = new OrderService();
            var productie = new ProductieService();
            var account = new AccountmanagerService();
            var expeditie = new ExpeditieService();
            var factuur = new FacturatieService();

            Assert.True(orderService.ValideerEnAccordeer(order));
            Assert.True(productie.ControleerMaterialenEnStart(order));
            Assert.True(account.ControleerEindproduct(order));

            expeditie.VerzendProduct(order);
            factuur.Factureer(order);
        }
    }
}
