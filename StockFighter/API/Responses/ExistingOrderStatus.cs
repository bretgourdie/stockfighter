using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.API.Responses
{
    /// <summary>
    /// Deserialized response for the status of an existing order.
    /// </summary>
    public class ExistingOrderStatus : AbstractOrderResponse<OrderType, OrderDirection>
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
        /// Creates a client-facing ExistingOrderStatus.
        /// </summary>
        /// <param name="existingOrderStatus">
        /// The deserialized existing order response from the StockFighter API.
        /// </param>
        internal ExistingOrderStatus(_existingOrderStatus existingOrderStatus) : base(existingOrderStatus)
        {
            this.direction = getDirection(existingOrderStatus.direction);
            this.orderType = getType(existingOrderStatus.orderType);
        }
    }
}
