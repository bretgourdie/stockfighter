using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.Requests
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
        public _orderRequest(OrderRequest orderRequest)
        {
            this.account = orderRequest.account;
            this.venue = orderRequest.venue;
            this.stock = orderRequest.stock;
            this.price = orderRequest.price;
            this.qty = orderRequest.qty;
            this.orderType = getOrderType(orderRequest.orderType);
            this.direction = getDirection(orderRequest.direction);
        }

        /// <summary>
        /// Returns stringified version of the supplied OrderDirection.
        /// </summary>
        /// <param name="direction">The direction of the order.</param>
        /// <returns>
        /// Returns a stringified OrderDirection for the API.
        /// </returns>
        private string getDirection(OrderDirection direction)
        {
            string sDirection = "";
            switch (direction)
            {
                case OrderDirection.Buy:
                    sDirection = "buy";
                    break;
                case OrderDirection.Sell:
                    sDirection = "sell";
                    break;
                default:
                    throw new NotImplementedException("Direction \""
                        + direction.ToString() + "\" not implemented.");
            }

            return sDirection;
        }

        /// <summary>
        /// Returns stringified version of the supplied OrderType.
        /// </summary>
        /// <param name="type">The type of the order.</param>
        /// <returns>A stringified OrderType for the API.</returns>
        private string getOrderType(OrderType type)
        {
            string sType = "";
            switch (type)
            {
                case OrderType.Limit:
                    sType = "limit";
                    break;
                case OrderType.Market:
                    sType = "market";
                    break;
                case OrderType.FillOrKill:
                    sType = "fill-or-kill";
                    break;
                case OrderType.ImmediateOrCancel:
                    sType = "immediate-or-cancel";
                    break;
                default:
                    throw new NotImplementedException("Type \""
                        + type.ToString() + "\" not implemented.");
            }

            return sType;
        }
    }
}
