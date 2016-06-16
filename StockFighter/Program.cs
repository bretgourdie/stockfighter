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
            var bStar = "***** ";
            var eStar = " *****";

            var level1 = new Level01_First_Steps(apiKey);
            Console.WriteLine(bStar + " Starting " + level1.ToString() + eStar);
            var solved = level1.Solve();

            if (solved)
            {
                Console.WriteLine(bStar + level1.ToString() + " was solved!" + eStar);
            }

        }
    }
}
