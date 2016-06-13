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
        /// Overrides the default authorization method to include the cookie declaration in the resource property.
        /// </summary>
        /// <param name="request">The RestRequest to authorize.</param>
        /// <param name="authorizationKey">Disregarded; using "api_key".</param>
        /// <param name="authorizationValue">The API Key to authenticate against.</param>
        /// <param name="authParameterType">Disregarded; not used.</param>
        /// <returns>Returns an authenticated RestRequest based off the GamemasterAPI spec.</returns>
        protected override RestRequest authorizeRequest(
            RestRequest request,
            string authorizationKey,
            string authorizationValue,
            ParameterType authParameterType)
        {
            request.Resource += " Cookie:api_key=" + authorizationValue;

            return request;
        }

        /// <summary>
        /// Overrides the default setRequestFormat method to not set the request format.
        /// </summary>
        /// <param name="request">The request to set the request format of.</param>
        /// <param name="requestFormat">Disregarded.</param>
        /// <returns>Returns the original RestRequest.</returns>
        protected override RestRequest setRequestFormat(RestRequest request, DataFormat requestFormat)
        {
            return request;
        }
    }
}
