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

        public bool IsGeldig => Aantal >= 1 && Aantal <= 3;
    }
}

