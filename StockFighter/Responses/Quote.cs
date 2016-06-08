using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.Responses
{
    /// <summary>
    /// Deserialized representation of a quote request.
    /// </summary>
    public class Quote : APIResponse
    {
        /// <summary>
        /// The stocks's symbol.
        /// </summary>
        public string symbol { get; set; }
        /// <summary>
        /// The venue of the stock.
        /// </summary>
        public string venue { get; set; }
        /// <summary>
        /// The best price currently bid for the stock.
        /// </summary>
        public int bid { get; set; }
        /// <summary>
        /// The best price currently offered for the stock.
        /// </summary>
        public int ask { get; set; }
        /// <summary>
        /// The aggregate size of all orders at the best bid.
        /// </summary>
        public int bidSize { get; set; }
        /// <summary>
        /// The aggregate size of all orders at the best ask.
        /// </summary>
        public int askSize { get; set; }
        /// <summary>
        /// The aggregate size of all bids.
        /// </summary>
        public int bidDepth { get; set; }
        /// <summary>
        /// The aggregate size of all asks.
        /// </summary>
        public int askDepth { get; set; }
        /// <summary>
        /// The price of the last trade.
        /// </summary>
        public int last { get; set; }
        /// <summary>
        /// The quantity of the last trade.
        /// </summary>
        public int lastSize { get; set; }
        /// <summary>
        /// The timestamp of the last trade.
        /// </summary>
        public DateTime lastTrade { get; set; }
        /// <summary>
        /// The timestamp the quote was last updated server-side.
        /// </summary>
        public DateTime quoteTime { get; set; }
    }
}
