using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StockFighter.Common;

namespace StockFighter.API.Responses
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
        internal AbstractOrderResponse(AbstractOrderResponse<string, string> concreteResponse) 
        {
            this.ok = concreteResponse.ok;
            this.symbol = concreteResponse.symbol;
            this.venue = concreteResponse.venue;
            this.originalQty = concreteResponse.originalQty;
            this.qty = concreteResponse.qty;
            this.price = concreteResponse.price;
            this.id = concreteResponse.id;
            this.account = concreteResponse.account;
            this.ts = concreteResponse.ts;
            this.fills = concreteResponse.fills;
            this.totalFilled = concreteResponse.totalFilled;
            this.open = concreteResponse.open;
        }

        /// <summary>
        /// Parameterless constructor for RestSharp.
        /// </summary>
        public AbstractOrderResponse() { }

        /// <summary>
        /// Translates the stringified order direction to an OrderDirection.
        /// </summary>
        /// <param name="direction">The direction of the order.</param>
        /// <returns>
        /// Returns the OrderDirection of the order direction.
        /// </returns>
        protected OrderDirection getDirection(string direction)
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
        protected OrderType getType(string type)
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
