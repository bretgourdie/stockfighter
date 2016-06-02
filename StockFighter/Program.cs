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
            var venue = "EYZEX";

            try
            {
                var stocks = APIWrapper.GetStocks(venue);

                Console.WriteLine("Stocks for venue \"" + venue + "\":");

                foreach (var stock in stocks.symbols)
                {
                    Console.WriteLine("\t" + stock.symbol + " (" + stock.name + ")");
                    
                    var orderbook = APIWrapper.GetOrderbook(venue, stock.symbol);

                    Console.WriteLine("\tBids: ");
                    foreach (var bid in orderbook.bids)
                    {
                        Console.WriteLine("\t\t" + bid.qty + " @ $" + bid.price);
                    }

                    Console.WriteLine("\n\tAsks: ");
                    foreach (var ask in orderbook.asks)
                    {
                        Console.WriteLine("\t\t" + ask.qty + " @ $" + ask.price);
                    }
                }
            }

            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey(true);
        }
    }
}
