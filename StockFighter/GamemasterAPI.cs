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
        protected override string url { get { return @"https://www.stockfighter.io/gm"; } }

        /// <summary>
        /// Authorization parameter name for GM API calls.
        /// </summary>
        protected override string authorizationParameterName { get { return @" Cookie:api_key="; } }

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

        /// <summary>
        /// Overridden to simplify the POST request.
        /// </summary>
        /// <param name="request">The RestRequest.</param>
        /// <param name="authorizationKey">The authorization parameter name.</param>
        /// <param name="authorizationValue">The authorization parameter value.</param>
        /// <param name="authParameterType">Disregarded.</param>
        /// <returns>Returns an authorized RestRequest.</returns>
        protected override RestRequest authorizeRequest(RestRequest request, string authorizationKey, string authorizationValue, ParameterType authParameterType)
        {
            request.Resource += authorizationKey + authorizationValue;

            return request;
        }

        /// <summary>
        /// Overridden to not add a parameter to the POST request.
        /// </summary>
        /// <param name="request">The RestRequest.</param>
        /// <param name="requestFormat">Disregarded.</param>
        /// <returns>Returns the original RestRequest.</returns>
        protected override RestRequest setRequestFormat(RestRequest request, DataFormat requestFormat)
        {
            return request;
        }
    }
}
