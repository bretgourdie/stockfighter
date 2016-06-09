using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;
using StockFighter.Responses;

namespace StockFighter
{
    /// <summary>
    /// Wraps Gamemaster API functions.
    /// </summary>
    public class GamemasterAPI : AbstractAPI
    {
        /// <summary>
        /// URL for interfacing with the Gamemaster API.
        /// </summary>
        protected override string url { get { return @"https://ww.stockfighter.io/gm"; } }

        /// <summary>
        /// Initializes a dictionary of Gamemaster API commands and returns it.
        /// </summary>
        /// <returns>Returns an initialized dictionary of Gamemaster API commands.</returns>
        protected override Dictionary<Type, string> getCommandDictionary()
        {
            var dict = new Dictionary<Type, string>
            {
                {typeof(StartLevel), "/levels/{0}"}
            };

            return dict;
        }
    }
}
