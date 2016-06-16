using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using StockFighter.Solutions;

namespace StockFighter
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiKey = ConfigurationManager.AppSettings["apiKey"];

            var level1 = new Level01_First_Steps(apiKey);
            level1.Solve();
        }
    }
}
