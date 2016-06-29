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
    public class Level03_Sell_Side : ISolvable
    {
        /// <summary>
        /// The assigned APIKey for the level.
        /// </summary>
        protected string apiKey { get; set; }

        /// <summary>
        /// Creates an initialized Sell_Side solution using the specified API key.
        /// </summary>
        /// <param name="apiKey"></param>
        public Level03_Sell_Side(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public bool Solve()
        {
            var wrapper = new StockFighterAPI(this.apiKey);
            var gamemaster = new GamemasterAPI(this.apiKey);

            var solved = false;
            var remainingProfitToBeMade = 10000;
            var excessiveRiskMax = 1000;

            try
            {

            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return solved;
        }

        /// <summary>
        /// Return the name of the level.
        /// </summary>
        /// <returns>Returns the name of the level.</returns>
        public string ToString()
        {
            return "Level 03: Sell Side";
        }

        /// <summary>
        /// The level's name.
        /// </summary>
        public string LevelName
        {
            get { return "sell_side"; }
        }
    }
}
