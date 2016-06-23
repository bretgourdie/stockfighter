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
    /// The solution to the second level, Chock_A_Block.
    /// </summary>
    public class Level02_Chock_A_Block : ISolvable<Level02_Chock_A_Block>
    {
        protected string apiKey { get; set; }

        /// <summary>
        /// Creates an initialized Chock_A_Block solution using the specified API key.
        /// </summary>
        /// <param name="apiKey">The API key to use for th eleve.</param>
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

            var sharesToSell = 100000;
            var solved = false;

            return solved;
        }
    }
}
