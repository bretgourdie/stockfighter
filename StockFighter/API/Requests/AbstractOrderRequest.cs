using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StockFighter.Common;

namespace StockFighter.API.Requests
{
    /// <summary>
    /// Represents the base of an OrderRequest.
    /// Use to generate a concrete OrderRequest.
    /// </summary>
    public abstract class AbstractOrderRequest<OrderTypeT, OrderDirectionT> : APIRequest
    {
        /// <summary>
        /// The trading account you are trading for.
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// The venue to place the trade to.
        /// </summary>
        public string venue { get; set; }
        /// <summary>
        /// The stock symbol to place the trade in.
        /// </summary>
        public string stock { get; set; }
        /// <summary>
        /// The desired price. Ignored for market orders.
        /// </summary>
        public int price { get; set; }
        /// <summary>
        /// The desired quantity.
        /// </summary>
        public virtual int qty { get; set; }
        /// <summary>
        /// The order type. Use a concrete type when inheriting.
        /// </summary>
        public abstract OrderTypeT orderType { get; set; }
        /// <summary>
        /// The order direction. Use a concrete type when inheriting.
        /// </summary>
        public abstract OrderDirectionT direction { get; set; }
    }
}
