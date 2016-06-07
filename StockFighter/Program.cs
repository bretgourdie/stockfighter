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
            var venue = @"FCMEX";
            var account = @"LPK74064170";

            try
            {
                var stocks = APIWrapper.GetStocks(venue);

                Console.WriteLine("Stocks for venue \"" + venue + "\":");

                foreach (var stock in stocks.symbols)
                {
                    Console.WriteLine("\t" + stock.symbol + " (" + stock.name + ")");

                    var quote = APIWrapper.GetQuote(venue, stock.symbol);

                    Console.WriteLine("\tQuote:");
                    Console.WriteLine("\t\tAsk: " + quote.ask);
                    Console.WriteLine("\t\tBid: " + quote.bid);
                    Console.WriteLine("\t\tTime: " + quote.quoteTime);
                    Console.WriteLine("\t\tLast trade at: " + quote.lastTrade);
                    Console.WriteLine("\n");

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

                    // Attempt to buy 100 shares
                    var orderRequest = new OrderRequest(
                        account,
                        venue,
                        stock.symbol,
                        0,
                        100,
                        OrderDirection.Buy,
                        OrderType.Market);

                    var orderResponse = APIWrapper.PostOrder(orderRequest);
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
