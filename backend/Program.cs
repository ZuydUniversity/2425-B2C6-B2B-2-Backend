using backend.Models;
using System;

namespace backend
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var simulator = new Simulator();
            simulator.StartInteractieveSimulatie();
        }
    }
}
