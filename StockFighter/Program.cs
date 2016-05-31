using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter
{
    class Program
    {
        static void Main(string[] args)
        {
            var venue = "AIBEX";
            Console.WriteLine(venue + " is up? " + APIWrapper.CheckVenue(venue));
            Console.WriteLine("Garbage is up? " + APIWrapper.CheckVenue("garbage"));

            Console.ReadKey(true);
        }
    }
}
