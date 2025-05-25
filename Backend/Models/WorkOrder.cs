using System;

namespace Backend.Models
{
	public class WorkOrder
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public Order RelatedOrder { get; set; }
		public DateTime DeliveryDate { get; set; }
		public string ProductionLine { get; set; }
	}
}
