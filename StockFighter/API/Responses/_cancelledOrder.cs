using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.API.Responses
{
    /// <summary>
    /// Internal representation of a CancelledOrder.
    /// </summary>
    internal class _cancelledOrder : AbstractOrderResponse<string, string>
    {
        /// <summary>
        /// A stringified version of the order's direction.
        /// </summary>
        public override string direction { get; set; }
        /// <summary>
        /// A stringified version of the order's type.
        /// </summary>
        public override string orderType { get; set; }
    }
}
