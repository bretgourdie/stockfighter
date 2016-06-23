using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.Solutions
{
    /// <summary>
    /// The solution to the second level, Chock_A_Block.
    /// </summary>
    public class Level02_Chock_A_Block : ISolvable<Level02_Chock_A_Block>
    {
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

        }
    }
}
