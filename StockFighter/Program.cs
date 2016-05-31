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

            try
            {
                var stocks = APIWrapper.GetStocks(venue);

                Console.WriteLine("Stocks for venue \"" + venue + "\":");

                foreach (var stock in stocks.symbols)
                {
                    Console.WriteLine("\tStock: " + stock.name + " (" + stock.symbol + ")");
                }
            }

            catch (ArgumentException)
            {
                Console.WriteLine("No stocks found for venue \"" + venue + "\".");
            }

            Console.ReadKey(true);
        }
    }
}
