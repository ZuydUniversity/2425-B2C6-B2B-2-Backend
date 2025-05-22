
using Xunit;
using backend.Models;
using backend.Services;

namespace backend.Tests.Services
{
    public class ProductieServiceTests
    {
        [Fact]
        public void StartProductie_AlleenAlsOrderGeaccordeerd()
        {
            var order = new Order { KlantNaam = "Test", Aantal = 1, Geaccordeerd = true };
            var productie = new ProductieService();

            var result = productie.ControleerMaterialenEnStart(order);

            Assert.True(result);
        }

        [Fact]
        public void StartProductie_FaaltZonderAccordering()
        {
            var order = new Order { KlantNaam = "Test", Aantal = 1, Geaccordeerd = false };
            var productie = new ProductieService();

            var result = productie.ControleerMaterialenEnStart(order);

            Assert.False(result);
        }
    }
}
