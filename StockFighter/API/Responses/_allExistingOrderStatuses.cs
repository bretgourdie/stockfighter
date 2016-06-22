using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.API.Responses
{
    /// <summary>
    /// Internal deserialized response from querying all existing orders.
    /// </summary>
    internal class _allExistingOrderStatuses : Common.APIResponse
    {
        /// <summary>
        /// The venue of the orders.
        /// </summary>
        public string venue { get; set; }
        /// <summary>
        /// The status of each order.
        /// </summary>
        public List<_existingOrderStatus> orders { get; set; }
    }
}
