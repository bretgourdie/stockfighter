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
        public bool Solve()
        {
            throw new NotImplementedException();
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
