using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.Responses
{
    /// <summary>
    /// Base for all types of OrderResponse.
    /// </summary>
    public abstract class AbstractOrderResponse<OrderTypeT, OrderDirectionT> : APIResponse
    {
        /// <summary>
        /// The stock's symbol.
        /// </summary>
        public string symbol { get; set; }
        /// <summary>
        /// The stock's venue.
        /// </summary>
        public string venue { get; set; }
        /// <summary>
        /// The original quantity of the order.
        /// </summary>
        public int originalQty { get; set; }
        /// <summary>
        /// The quantity left outstanding from executed fills.
        /// </summary>
        public int qty { get; set; }
        /// <summary>
        /// The price of the order's fill. May not match the fills!
        /// </summary>
        public int price { get; set; }
        /// <summary>
        /// A guaranteed unique number on this venue.
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// The account used to execute the trade.
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// The time the order was received.
        /// </summary>
        public DateTime ts { get; set; }
        /// <summary>
        /// A list of fills for the order.
        /// </summary>
        public List<Fill> fills { get; set; }
        /// <summary>
        /// The amount of quantity that was filled.
        /// </summary>
        public int totalFilled { get; set; }
        /// <summary>
        /// If the order can still be filled.
        /// </summary>
        public bool open { get; set; }
        /// <summary>
        /// The type of the order. Override with a concrete type.
        /// </summary>
        public abstract OrderTypeT orderType { get; set; }
        /// <summary>
        /// The direction of the order. Override with a concrete type.
        /// </summary>
        public abstract OrderDirectionT direction { get; set; }

        /// <summary>
        /// Don't allow AbstractOrderResponse to be inherited from 
        /// outside of the assembly.
        /// </summary>
        internal AbstractOrderResponse() { }

        /// <summary>
        /// Representation of a fill from an OrderResponse.
        /// </summary>
        public class Fill
        {
            /// <summary>
            /// The price the order was filled at.
            /// </summary>
            public int price { get; set; }
            /// <summary>
            /// The amount filled.
            /// </summary>
            public int qty { get; set; }
            /// <summary>
            /// The time the fill occurred.
            /// </summary>
            public DateTime ts { get; set; }
        }
    }
}
