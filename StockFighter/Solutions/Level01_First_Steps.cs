using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StockFighter.API;
using StockFighter.Gamemaster;

namespace StockFighter.Solutions
{
    /// <summary>
    /// The solution to the first level, First_Steps.
    /// </summary>
    public class Level01_First_Steps
    {
        /// <summary>
        /// The assigned APIKey for the level
        /// </summary>
        public string APIKey { get; private set; }

        public Level01_First_Steps(string apiKey)
        {

        }

        public bool Solve()
        {
            var wrapper = new StockFighterAPI();
            var gamemaster = new GamemasterAPI();

            try
            {
                Console.Write("Starting level... ");
                var levelInfo = gamemaster.StartLevel("first_steps");
                Console.WriteLine(levelInfo.ok + "!");

                var account = levelInfo.account;
                var instanceId = levelInfo.instanceId;
                var venues = levelInfo.venues;

                Console.WriteLine(
                    "instanceId: " + instanceId
                    + "account: " + account);
                

                foreach (var venue in venues)
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
                        var orderRequest = new API.Requests.OrderRequest(
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
            }

            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
