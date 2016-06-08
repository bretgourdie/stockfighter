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
            var venue = @"PHKEX";
            var account = @"SH80055215";
            var wrapper = new APIWrapper();

            try
            {
                var stocks = wrapper.GetStocks(venue);

                Console.WriteLine("Stocks for venue \"" + venue + "\":");

                foreach (var stock in stocks.symbols)
                {
                    Console.WriteLine("\t" + stock.symbol + " (" + stock.name + ")");

                    var quote = wrapper.GetQuote(venue, stock.symbol);

                    Console.WriteLine("\tQuote:");
                    Console.WriteLine("\t\tAsk: " + quote.ask);
                    Console.WriteLine("\t\tBid: " + quote.bid);
                    Console.WriteLine("\t\tTime: " + quote.quoteTime);
                    Console.WriteLine("\t\tLast trade at: " + quote.lastTrade);
                    Console.WriteLine("\n");

                    var orderbook = wrapper.GetOrderbook(venue, stock.symbol);

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
                    var orderRequest = new Requests.OrderRequest(
                        account,
                        venue,
                        stock.symbol,
                        0,
                        100,
                        OrderDirection.Buy,
                        OrderType.Market);

                    var orderResponse = wrapper.PostOrder(orderRequest);
                    if (orderResponse != null && orderResponse.fills.Count > 0)
                    {
                        Console.WriteLine("\tFills:");

                        foreach (var fill in orderResponse.fills)
                        {
                            Console.WriteLine("\t\t" + fill.qty + " @ $" + fill.price);
                        }
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
