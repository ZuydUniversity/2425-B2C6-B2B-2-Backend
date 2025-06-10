using Backend.Models;

namespace Backend.Services
{
    public class PlannerService
    {
        public WorkOrder CreateWorkOrder(Order order, ProductionLine line)
        {
            return new WorkOrder
            {
                RelatedOrder = order,
                OrderId = order.Id,
                DeliveryDate = DateTime.Now.AddDays(7),
                ProductionLine = line.Name
            };
        }

        public void ReportToManagement()
        {
            Console.WriteLine("Planner rapporteert bezettingsgraad aan het management.");
        }
    }
}
