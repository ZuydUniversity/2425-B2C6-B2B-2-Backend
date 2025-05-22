
using backend.Models;

namespace backend.Services
{
    public class OrderService
    {
        public bool ValideerEnAccordeer(Order order)
        {
            if (!order.IsGeldig)
                return false;

            order.Geaccordeerd = true;
            return true;
        }
    }
}
