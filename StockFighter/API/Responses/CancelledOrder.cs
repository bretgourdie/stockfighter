using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.API.Responses
{
    /// <summary>
    /// A client-facing version of a CancelledOrder.
    /// </summary>
    public class CancelledOrder : AbstractOrderResponse<OrderType, OrderDirection>
    {
        /// <summary>
        /// The direction of the order.
        /// </summary>
        public override OrderDirection direction { get; set; }
        /// <summary>
        /// The type of order.
        /// </summary>
        public override OrderType orderType { get; set; }

        /// <summary>
        /// Creates a client-facing CancelledOrder.
        /// </summary>
        /// <param name="cancelledOrder">The deserialized cancelled order from the API.</param>
        internal CancelledOrder(_cancelledOrder cancelledOrder) : base(cancelledOrder)
        {
            this.direction = getDirection(cancelledOrder.direction);
            this.orderType = getType(cancelledOrder.orderType);
        }
    }
}
