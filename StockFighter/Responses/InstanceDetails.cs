using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter.Responses
{
    public class InstanceDetails : APIResponse
    {
        public Details details { get; set; }
        public bool done { get; set; }
        public int id { get; set; }
        public string state { get; set; }

        public class Details
        {
            public int endOfTheWorldDay { get; set; }
            public int tradingDay { get; set; }
        }
    }
}
