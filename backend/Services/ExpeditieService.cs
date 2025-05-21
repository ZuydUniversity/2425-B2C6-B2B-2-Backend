
using backend.Models;
using System;

namespace backend.Services
{
    public class ExpeditieService
    {
        public void VerzendProduct(Order order)
        {
            Console.WriteLine($"Verzending gestart voor order van {order.KlantNaam}");
        }
    }
}
