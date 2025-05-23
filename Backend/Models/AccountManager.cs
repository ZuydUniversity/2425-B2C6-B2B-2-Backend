using System;

namespace Backend.Models
{
    public class AccountManager
    {
        public bool ApproveOrder(Order order)
        {
            if (!order.IsValid || !order.IsComplete)
            {
                WarnCustomer(order.CustomerName);
                Console.WriteLine("Order kan niet worden goedgekeurd: onvolledig of ongeldig.");
                return false;
            }

            order.IsApproved = true;
            order.Status = OrderStatus.Approved;
            Console.WriteLine($"Order van {order.CustomerName} is geaccordeerd.");
            return true;
        }

        public bool CheckOrderCompleteness(Order order)
        {
            if (!order.IsValid || !order.IsComplete)
            {
                WarnCustomer(order.CustomerName);
                throw new InvalidOperationException("Orderformulier is ongeldig of onvolledig.");
            }

            Console.WriteLine("Orderformulier is compleet en geldig.");
            return true;
        }

        public void WarnCustomer(string customerName)
        {
            Console.WriteLine($"Waarschuwing: Orderformulier van {customerName} is onvolledig of ongeldig.");
        }

        public void AddToWorkInProgress(Order order)
        {
            order.Status = OrderStatus.InProduction;
            Console.WriteLine($"Order {order.Id} toegevoegd aan 'Werk Onder Handen'.");
        }

        public void CheckOffWorkInProgress(Order order)
        {
            if (order.Status != OrderStatus.InProduction)
                throw new InvalidOperationException("Order is niet in productie en kan niet worden afgevinkt.");

            order.Status = OrderStatus.Ready;
            Console.WriteLine($"Order {order.Id} afgehandeld in 'Werk Onder Handen'.");
        }

        public void WarnPlantManager()
        {
            Console.WriteLine("Waarschuwing: kwaliteitsprobleem gedetecteerd. Plantmanager is geïnformeerd.");
        }

        public void SignOrder(Order order)
        {
            if (!order.IsValid || !order.IsComplete)
                throw new InvalidOperationException("Kan order niet ondertekenen: onvolledig of ongeldig.");

            order.IsSignedByAccountManager = true;
            Console.WriteLine($"Order {order.Id} is ondertekend door accountmanager.");
        }

        public void CheckProductQuality(Order order)
        {
            if (order.HasQualityIssues)
            {
                WarnPlantManager();
                throw new InvalidOperationException("Kwaliteitscontrole mislukt voor order " + order.Id);
            }

            Console.WriteLine($"Order {order.Id} is goedgekeurd na kwaliteitscontrole.");
        }

        public void DeliverOrder(Order order)
        {
            if (order.Status != OrderStatus.Ready)
                throw new InvalidOperationException("Order is niet klaar voor levering.");

            order.Status = OrderStatus.Shipped;
            Console.WriteLine($"Order {order.Id} is geleverd aan klant {order.CustomerName}.");
        }
    }
}
