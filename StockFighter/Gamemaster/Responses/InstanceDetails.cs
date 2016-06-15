using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StockFighter.Common;

namespace StockFighter.Gamemaster.Responses
{
    /// <summary>
    /// Deserialized response about the current instance's details.
    /// </summary>
    public class InstanceDetails : APIResponse
    {
        /// <summary>
        /// Information about simulation timing.
        /// </summary>
        public Details details { get; set; }
        /// <summary>
        /// Is the level timed out?
        /// </summary>
        public bool done { get; set; }
        /// <summary>
        /// The instance id.
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Is the market open?
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// Explains information about how far along the simulation is.
        /// </summary>
        public class Details
        {
            /// <summary>
            /// The day that the simulation ends.
            /// </summary>
            public int endOfTheWorldDay { get; set; }
            /// <summary>
            /// The current day in the simulation.
            /// </summary>
            public int tradingDay { get; set; }
        }
    }
}
