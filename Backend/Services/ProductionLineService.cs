using Backend.Models;

namespace Backend.Services
{
    public class ProductionLineService
    {
        public void ProcessWorkOrder(WorkOrder workOrder)
        {
            Console.WriteLine($"Productielijn {workOrder.ProductionLine} verwerkt werkorder voor order {workOrder.OrderId}.");
        }

        public void ReportOccupancy(List<WorkOrder> workOrders)
        {
            Console.WriteLine($"Aantal werkorders ingepland: {workOrders.Count}");
        }
    }
}
