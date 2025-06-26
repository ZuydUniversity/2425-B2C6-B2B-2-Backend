namespace API.Models
{
    public class Planning
    {
        /// <summary>
        /// Planning, bevat een order die gepland is op een bepaalde productielijn.
        /// Veranderd de status van de order in 'Planned'
        /// </summary>

        public int Id { get; set; }
        public DateTime PlannedDate { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductionLineId { get; set; }
        public ProductionLine? ProductionLine { get; set; }
    }
}
