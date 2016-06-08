using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.Responses
{
    /// <summary>
    /// Representation of a fill from an OrderResponse.
    /// </summary>
    public class Fill
    {
        /// <summary>
        /// The price the order was filled at.
        /// </summary>
        public int price { get; set; }
        /// <summary>
        /// The amount filled.
        /// </summary>
        public int qty { get; set; }
        /// <summary>
        /// The time the fill occurred.
        /// </summary>
        public DateTime ts { get; set; }
    }
}
