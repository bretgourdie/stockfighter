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

        /// <summary>
        /// Internal constructor for strongly-typed request to internal request.
        /// </summary>
        /// <param name="orderRequest"></param>
        internal AbstractOrderRequest(AbstractOrderRequest<OrderType, OrderDirection> orderRequest)
        {
            this.account = orderRequest.account;
            this.venue = orderRequest.venue;
            this.stock = orderRequest.stock;
            this.price = orderRequest.price;
            this.qty = orderRequest.qty;
        }

        /// <summary>
        /// Constructor for strongly-typed order requests.
        /// </summary>
        /// <param name="account">The account to order through.</param>
        /// <param name="venue">The venue for the stock.</param>
        /// <param name="stock">The stock's symbol.</param>
        /// <param name="price">The desired price.</param>
        /// <param name="qty">The desired quantity.</param>
        /// <param name="direction">The direction of the order.</param>
        /// <param name="ordertype">The type of order.</param>
        internal AbstractOrderRequest(
            string account,
            string venue,
            string stock,
            int price,
            int qty,
            OrderDirectionT direction,
            OrderTypeT ordertype)
        {
            this.account = account;
            this.venue = venue;
            this.stock = stock;
            this.price = price;
            this.qty = qty;
            this.direction = direction;
            this.orderType = ordertype;
        }

        /// <summary>
        /// Returns stringified version of the supplied OrderDirection.
        /// </summary>
        /// <param name="direction">The direction of the order.</param>
        /// <returns>
        /// Returns a stringified OrderDirection for the API.
        /// </returns>
        protected string getDirection(OrderDirection direction)
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
        protected string getOrderType(OrderType type)
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
