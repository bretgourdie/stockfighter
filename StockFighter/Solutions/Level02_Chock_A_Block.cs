using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StockFighter.API;
using StockFighter.API.Requests;
using StockFighter.Gamemaster;

namespace StockFighter.Solutions
{
    /// <summary>
    /// The solution to the second level, Chock_A_Block.
    /// </summary>
    public class Level02_Chock_A_Block : ISolvable
    {
        protected string apiKey { get; set; }

        /// <summary>
        /// Creates an initialized Chock_A_Block solution using the specified API key.
        /// </summary>
        /// <param name="apiKey">The API key to use for the level.</param>
        public Level02_Chock_A_Block(string apiKey)
        {
            this.apiKey = apiKey;
        }
        /// <summary>
        /// Return the name of the level.
        /// </summary>
        /// <returns>Returns the name of the level.</returns>
        public override string ToString()
        {
            return "Level 02: Chock A Block";
        }

        /// <summary>
        /// The level's name.
        /// </summary>
        public string LevelName
        {
            get
            {
                return "chock_a_block";
            }
        }

        /// <summary>
        /// Solves the level Chock_A_Block.
        /// </summary>
        /// <returns>Returns if the level was solved or not.</returns>
        public bool Solve()
        {
            var wrapper = new StockFighterAPI(this.apiKey);
            var gamemaster = new GamemasterAPI(this.apiKey);

            var sharesToBuy = 100000;
            var solved = false;

            try
            {
                Console.Write("Starting level... ");
                var levelInfo = gamemaster.StartLevel(this.LevelName);
                Console.WriteLine(levelInfo.ok + "!");

                var account = levelInfo.account;
                var instanceId = levelInfo.instanceId;

                foreach (var venue in levelInfo.venues)
                {
                    Console.WriteLine("Venue \"" + venue + "\":");
                    var stocks = wrapper.GetStocks(venue);

                    foreach(var stock in stocks.symbols)
                    {
                        Console.WriteLine("Stock \"" + stock.name + "\" (" + stock.symbol + "):");
                        var buyingInterval = 100;
                        
                        while (sharesToBuy > 0)
                        {
                            Console.WriteLine("Shares left to buy: " + sharesToBuy);

                            var quantity = Math.Min(sharesToBuy, buyingInterval);

                            Console.Write("Attempting to order " + quantity + " shares... ");

                            var order = new OrderRequest(
                                account,
                                venue,
                                stock.symbol,
                                0,
                                quantity,
                                OrderDirection.Buy,
                                OrderType.Market);

                            var response = wrapper.PostOrder(order);

                            var totalFilled = 0;
                            foreach (var fill in response.fills)
                            {
                                totalFilled += fill.qty;
                            }

                            Console.WriteLine(totalFilled + " filled!");

                            sharesToBuy -= totalFilled;
                        }

                        Console.WriteLine("All shares bought!");
                        solved = true;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return solved;
        }
    }
}
