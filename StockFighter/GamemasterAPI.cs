using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;
using StockFighter.Responses;
using StockFighter.Requests;

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
                {typeof(StartedLevel), "/levels/{0}"}
            };

            return dict;
        }

        /// <summary>
        /// Start the specified level.
        /// </summary>
        /// <param name="levelName">The name of the level to start.</param>
        /// <returns>Returns information about the started level.</returns>
        public StartedLevel StartLevel(string levelName)
        {
            var startedLevel = postResponse<StartedLevel>(null, ParameterType.Cookie, levelName);

            if (startedLevel == null)
            {
                throw new ArgumentException("Unable to start the level.");
            }
            return startedLevel;
        }
    }
}
