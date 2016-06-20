using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.API.Requests
{
    /// <summary>
    /// Deserialized representation of an Order post.
    /// </summary>
    internal class _orderRequest : AbstractOrderRequest<string, string>
    {
        /// <summary>
        /// Whether you want to buy or sell.
        /// </summary>
        /// <remarks>
        /// To bid, use "buy". To ask, use "sell".
        /// </remarks>
        public override string direction { get; set; }
        /// <summary>
        /// The order type.
        /// </summary>
        public override string orderType { get; set; }

        /// <summary>
        /// Translates a client's OrderRequest to an API-ready _orderRequest.
        /// </summary>
        /// <param name="orderRequest">The client's order request.</param>
        internal _orderRequest(OrderRequest orderRequest) : base(orderRequest)
        {
            this.orderType = getOrderType(orderRequest.orderType);
            this.direction = getDirection(orderRequest.direction);
        }

    }
}
