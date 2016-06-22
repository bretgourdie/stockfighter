using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.API.Responses
{
    /// <summary>
    /// Client-facing response from querying all existing orders.
    /// </summary>
    public class AllExistingOrderStatuses : Common.APIResponse
    {
        /// <summary>
        /// The venue of the orders.
        /// </summary>
        public string venue { get; set; }
        /// <summary>
        /// The status of each order.
        /// </summary>
        public List<ExistingOrderStatus> orders { get; set; }
    }
}
