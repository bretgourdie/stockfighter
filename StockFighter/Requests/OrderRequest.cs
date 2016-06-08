using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.Requests
{
    /// <summary>
    /// User-facing representation of an Order post.
    /// </summary>
    public class OrderRequest : AbstractOrderRequest<OrderType, OrderDirection>
    {
        /// <summary>
        /// The type of the order request.
        /// </summary>
        public override OrderType orderType { get; set; }
        /// <summary>
        /// The direction of the order request.
        /// </summary>
        public override OrderDirection direction { get; set; }

        /// <summary>
        /// Creates an OrderRequest.
        /// Use the APIWrapper to post it.
        /// </summary>
        /// <param name="account">
        /// The account to purchase the order with.
        /// </param>
        /// <param name="venue">The venue of the stock.</param>
        /// <param name="stock">The stock's symbol.</param>
        /// <param name="price">The price to buy/sell at.</param>
        /// <param name="qty">The amount of stock to buy/sell.</param>
        /// <param name="direction">
        /// The decision to buy or sell the stock.
        /// </param>
        /// <param name="ordertype">
        /// The type of order to put.
        /// </param>
        public OrderRequest(
            string account,
            string venue,
            string stock,
            int price,
            int qty,
            OrderDirection direction,
            OrderType ordertype)
        {
            this.account = account;
            this.venue = venue;
            this.stock = stock;
            this.price = price;
            this.qty = qty;
            this.direction = direction;
            this.orderType = ordertype;
        }
    }
}
