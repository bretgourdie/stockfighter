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
    public class Level01_First_Steps : ISolvable
    {
        /// <summary>
        /// The assigned APIKey for the level
        /// </summary>
        protected string apiKey { get; set; }

        /// <summary>
        /// Creates an initialized First_Steps solution using the specified API key.
        /// </summary>
        /// <param name="apiKey">The API key to use for the level.</param>
        public Level01_First_Steps(string apiKey)
        {
            this.apiKey = apiKey;
        }

        /// <summary>
        /// Return the name of the level.
        /// </summary>
        /// <returns>Returns the name of the level.</returns>
        public override string ToString()
        {
            return "Level 01: First Steps";
        }

        public string LevelName
        {
            get
            {
                return "first_steps";
            }
        }

        /// <summary>
        /// Solves the level First_Steps.
        /// </summary>
        /// <returns>Returns if the level was solved or not.</returns>
        public bool Solve()
        {
            var wrapper = new StockFighterAPI(this.apiKey);
            var gamemaster = new GamemasterAPI(this.apiKey);

            var targetNumberOfShares = 100;
            var totalBought = 0;
            var solved = false;

            try
            {
                Console.Write("Starting level... ");
                var levelInfo = gamemaster.StartLevel(this.LevelName);
                Console.WriteLine(levelInfo.ok + "!");

                var account = levelInfo.account;
                var instanceId = levelInfo.instanceId;
                var venues = levelInfo.venues;

                Console.WriteLine(
                    "instanceId: " + instanceId
                    + "\naccount: " + account);
                

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

                        Console.Write("\tAttempting to buy "
                        + targetNumberOfShares
                        + " shares of \"" + stock.symbol + "\"... ");

                        while (totalBought < targetNumberOfShares)
                        {
                            // Attempt to buy targetNumberOfShares shares
                            var orderRequest = new API.Requests.OrderRequest(
                                account,
                                venue,
                                stock.symbol,
                                0,
                                targetNumberOfShares - totalBought,
                                OrderDirection.Buy,
                                OrderType.Market);

                            var orderResponse = wrapper.PostOrder(orderRequest);
                            if (orderResponse != null && orderResponse.fills.Count > 0)
                            {
                                Console.WriteLine("order accepted!");

                                var orderCheck = wrapper.GetOrderStatus(
                                    orderResponse.id, 
                                    orderResponse.venue, 
                                    orderResponse.symbol);

                                var totalFilled = 0;

                                foreach (var fill in orderResponse.fills)
                                {
                                    Console.WriteLine("\tFills:");
                                    Console.WriteLine("\t\t" + fill.qty + " @ $" + fill.price);
                                    totalFilled += fill.qty;
                                }

                                Console.WriteLine("\tBought " + totalFilled + " shares!");
                                Console.WriteLine("\n\tOrder is open? \"" + orderCheck.open + "\".\n");

                                totalBought += totalFilled;

                                if (totalBought == targetNumberOfShares)
                                {
                                    solved = true;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Couldn't buy!\n\t" + orderResponse.error);
                            }

                            Console.WriteLine("Progress so far: "
                                + totalBought
                                + "/"
                                + targetNumberOfShares
                                + " (" 
                                + (totalBought / targetNumberOfShares * 100)
                                + "%)");
                        }
                    }
                }
            }

            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return solved;
        }
    }
}
