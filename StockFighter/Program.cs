using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFighter
{
    class Program
    {
        static void Main(string[] args)
        {
            APIWrapper.CheckVenue("SZEEX");
            APIWrapper.CheckVenue("garbage");
        }
    }
}
