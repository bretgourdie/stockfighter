using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.Responses
{
    /// <summary>
    /// Deserialized response from a CheckVenue command.
    /// </summary>
    public class VenueHeartbeat : APIResponse
    {
        /// <summary>
        /// Venue that heartbeat was queried for.
        /// </summary>
        public string venue { get; set; }
    }
}
