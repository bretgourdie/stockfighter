using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp.Deserializers;
using StockFighter.Common;

namespace StockFighter.Gamemaster.Responses
{
    /// <summary>
    /// Response from starting a StockFighter level.
    /// </summary>
    public class StartedLevel : APIResponse
    {
        /// <summary>
        /// The account for the level.
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// The instance ID of the level.
        /// </summary>
        public int instanceId { get; set; }
        /// <summary>
        /// The list of instructions.
        /// </summary>
        public List<instruction> instructions { get; set; }
        /// <summary>
        /// The seconds per trading day.
        /// </summary>
        public int secondsPerTradingDay { get; set; }
        /// <summary>
        /// The list of tickers.
        /// </summary>
        public List<string> tickers { get; set; }
        /// <summary>
        /// The list of venues.
        /// </summary>
        public List<string> venues { get; set; }
        /// <summary>
        /// The list of balances.
        /// </summary>
        public List<balance> balances { get; set; }
        
        /// <summary>
        /// An instruction for a level.
        /// </summary>
        public class instruction
        {
            /// <summary>
            /// The text for one instruction.
            /// </summary>
            public string Instructions { get; set; }
            /// <summary>
            /// The type of order.
            /// </summary>
            [DeserializeAs(Name = "Order Types")]
            public string OrderTypes { get; set; }
        }

        /// <summary>
        /// A balance for a level.
        /// </summary>
        public class balance
        {
            /// <summary>
            /// The balance in United States dollars.
            /// </summary>
            public int USD { get; set; }
        }
    }
}
