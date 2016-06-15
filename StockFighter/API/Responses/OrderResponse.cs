using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StockFighter.Common;

namespace StockFighter.API.Responses
{
    /// <summary>
    /// A client-facing version of an OrderResponse.
    /// </summary>
    public class OrderResponse : AbstractOrderResponse<OrderType, OrderDirection>
    {
        /// <summary>
        /// The order's direction.
        /// </summary>
        public override OrderDirection direction { get; set; }
        /// <summary>
        /// The order's type.
        /// </summary>
        public override OrderType orderType { get; set; }

        /// <summary>
        /// Creates a client-facing OrderResponse.
        /// </summary>
        /// <param name="orderResponse">
        /// The deserialized order response from the APIWrapper.
        /// </param>
        internal OrderResponse(_orderResponse orderResponse)
        {
            this.ok = orderResponse.ok;
            this.symbol = orderResponse.symbol;
            this.venue = orderResponse.venue;
            this.direction = getDirection(orderResponse.direction);
            this.originalQty = orderResponse.originalQty;
            this.qty = orderResponse.qty;
            this.price = orderResponse.price;
            this.orderType = getType(orderResponse.orderType);
            this.id = orderResponse.id;
            this.account = orderResponse.account;
            this.ts = orderResponse.ts;
            this.fills = orderResponse.fills;
            this.totalFilled = orderResponse.totalFilled;
            this.open = orderResponse.open;
        }

        /// <summary>
        /// Translates the stringified order direction to an OrderDirection.
        /// </summary>
        /// <param name="direction">The direction of the order.</param>
        /// <returns>
        /// Returns the OrderDirection of the order direction.
        /// </returns>
        private OrderDirection getDirection(string direction)
        {
            OrderDirection eDirection;

            switch (direction)
            {
                case "buy":
                    eDirection = OrderDirection.Buy;
                    break;
                case "sell":
                    eDirection = OrderDirection.Sell;
                    break;
                default:
                    throw new NotImplementedException("Direction \""
                        + direction + "\" was not implemented.");
            }

            return eDirection;
        }

        /// <summary>
        /// Translates the stringified order type to an OrderType.
        /// </summary>
        /// <param name="type">The type of the order.</param>
        /// <returns>Returns the OrderType of the order type.</returns>
        private OrderType getType(string type)
        {
            OrderType eType;

            switch (type)
            {
                case "limit":
                    eType = OrderType.Limit;
                    break;
                case "market":
                    eType = OrderType.Market;
                    break;
                case "fill-or-kill":
                    eType = OrderType.FillOrKill;
                    break;
                case "immediate-or-cancel":
                    eType = OrderType.ImmediateOrCancel;
                    break;
                default:
                    throw new NotImplementedException("Type \""
                        + type + "\" was not implemented.");
            }

            return eType;
        }
    }
}
