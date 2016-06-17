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
        internal OrderResponse(_orderResponse orderResponse) : base(orderResponse)
        {
            this.direction = getDirection(orderResponse.direction);
            this.orderType = getType(orderResponse.orderType);
        }

        
    }
}
