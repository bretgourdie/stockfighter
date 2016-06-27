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

            var levelCollection = new List<ISolvable>()
            {
                //new Level01_First_Steps(apiKey),
                //new Level02_Chock_A_Block(apiKey),
                new Level03_Sell_Side(apiKey)
            };

            foreach (var level in levelCollection)
            {
                Console.WriteLine(bStar + " Starting " + level.ToString() + eStar);
                var solved = level.Solve();

                if (solved)
                {
                    Console.WriteLine(bStar + level.ToString() + " was solved!" + eStar);
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
        }
    }
}
