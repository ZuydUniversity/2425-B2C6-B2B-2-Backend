using System;

namespace backend.Models
{
    public class Order
    {
        public string KlantNaam { get; set; } = string.Empty;
        public MotorType Type { get; set; }
        public int Aantal { get; set; }
        public DateTime Besteldatum { get; set; }
        public bool Geaccordeerd { get; set; } = false;
        public OrderStatus Status { get; set; } = OrderStatus.Nieuw;


        public bool IsGeldig => Aantal >= 1 && Aantal <= 3;
    }
    public enum OrderStatus
    {
        Nieuw,
        Goedgekeurd,
        Geweigerd,
        InPlanning,
        InProductie,
        Gereed,
        Verzendklaar,
        Verzend,
        Gefactureerd
    }

}
