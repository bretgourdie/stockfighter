﻿using System;
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

        /// <summary>
        /// Creates a forward-facing set of existing orders from the internal representation.
        /// </summary>
        /// <param name="existingOrderStatuses">The API response of All Existing Orders.</param>
        internal AllExistingOrderStatuses(_allExistingOrderStatuses existingOrderStatuses)
        {
            this.venue = existingOrderStatuses.venue;

            this.orders = new List<ExistingOrderStatus>();

            foreach (var existingOrder in existingOrderStatuses.orders)
            {
                orders.Add(new ExistingOrderStatus(existingOrder));
            }
        }
    }
}
