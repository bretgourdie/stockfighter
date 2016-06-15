using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.API
{
    /// <summary>
    /// Type of order.
    /// </summary>
    public enum OrderType 
    {
        /// <summary>
        /// <para>
        /// The most common order.
        /// </para>
        /// <para>
        /// Immediately matches any orders offering prices "as good or better
        /// as the ones listed in the order."
        /// Has any unmatched portion of the order rests on the orderbook.
        /// </para>
        /// <para>
        /// Good until cancelled.
        /// </para>
        /// </summary>
        Limit,
        /// <summary>
        /// Executes immediately regardless of price.
        /// </summary>
        Market,
        /// <summary>
        /// Limit order for immediate execution on an all-or-nothing basis.
        /// The order may be accepted ("ok":true) but not receive any fills
        /// ("open":false).
        /// </summary>
        FillOrKill,
        /// <summary>
        /// A <c ref="OrderType.FillOrKill"/> that accepts partial execution.
        /// </summary>
        ImmediateOrCancel
    }
}
