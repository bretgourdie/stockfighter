using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StockFighter.Common;

namespace StockFighter.API.Responses
{
    /// <summary>
    /// Deserialized response from a <c ref="GetOrderbook"/> command.
    /// </summary>
    public class Orderbook : APIResponse
    {
        /// <summary>
        /// The venue of the stock.
        /// </summary>
        public string venue { get; set; }
        /// <summary>
        /// The stock's symbol.
        /// </summary>
        public string symbol { get; set; }
        /// <summary>
        /// The list of bids (buys) for the stock.
        /// </summary>
        public List<Orderbook.Request> bids { get; set; }
        /// <summary>
        /// The list of asks (sells) for the stock.
        /// </summary>
        public List<Orderbook.Request> asks { get; set; }
        /// <summary>
        /// The timestamp the orderbook was retrieved.
        /// </summary>
        public DateTime ts { get; set; }

        /// <summary>
        /// Deserialized representation of an orderbook request (bid or ask).
        /// </summary>
        public class Request
        {
            /// <summary>
            /// The price of the request.
            /// </summary>
            public decimal price { get; set; }
            /// <summary>
            /// The quantity of the request.
            /// </summary>
            public int qty { get; set; }
            /// <summary>
            /// Is this a request to buy (bid) or sell (ask)?
            /// </summary>
            public bool isBuy { get; set; }
        }
    }
}
