using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.Responses
{
    /// <summary>
    /// Deserialized response from a GetStocks command.
    /// </summary>
    public class VenueStocks : APIResponse
    {
        /// <summary>
        /// List of stocks in the venue.
        /// </summary>
        public List<VenueStock> symbols { get; set; }
    
        /// <summary>
        /// Deserialized stocks from a GetStocks command.
        /// </summary>
        public class VenueStock : APIResponse
        {
            /// <summary>
            /// The name of the stock.
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// The symbol of the stock.
            /// </summary>
            public string symbol { get; set; }
        }
    }
}
