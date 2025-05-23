
using Backend.Models;

namespace Backend.Services
{
    public class OrderService
    {
        public bool ValidateAndApprove(Order order)
        {
            if (!order.IsValid)
                return false;

            order.IsApproved = true;
            order.Status = OrderStatus.Approved;
            return true;
        }
    }
}
