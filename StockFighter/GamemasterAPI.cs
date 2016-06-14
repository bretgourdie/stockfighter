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
        protected override string authorizationParameterName { get { return @"api_key"; } }

        /// <summary>
        /// Initializes a dictionary of Gamemaster API commands and returns it.
        /// </summary>
        /// <returns>Returns an initialized dictionary of Gamemaster API commands.</returns>
        protected override Dictionary<Type, string> getCommandDictionary()
        {
            var dict = new Dictionary<Type, string>
            {
                {typeof(StartedLevel), "/levels/{0}"},
                {typeof(RestartedLevel), "/instances/{0}/restart"},
                {typeof(StoppedLevel), "/instances/{0}/stop"},
                {typeof(ResumedLevel), "/instances/{0}/resume"}
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
        /// Restarts the specified instance.
        /// </summary>
        /// <param name="instanceId">The instance's ID to restart.</param>
        /// <returns>Returns information about the restarted instance.</returns>
        public RestartedLevel RestartLevel(int instanceId)
        {
            return setLevelPlayback<RestartedLevel>(instanceId);
        }

        /// <summary>
        /// Resumes the specified instance.
        /// </summary>
        /// <param name="instanceId">The instance's ID to resume.</param>
        /// <returns>Returns information about the resumed instance.</returns>
        public ResumedLevel ResumeLevel(int instanceId)
        {
            return setLevelPlayback<ResumedLevel>(instanceId);
        }

        /// <summary>
        /// Stops the specified instance.
        /// </summary>
        /// <param name="instanceId">The instance's ID to stop.</param>
        /// <returns>Returns information about the stopped instance.</returns>
        public StoppedLevel StopLevel(int instanceId)
        {
            return setLevelPlayback<StoppedLevel>(instanceId);
        }

        /// <summary>
        /// Helper method to reduce code in instance-affecting response methods.
        /// </summary>
        /// <typeparam name="T">The expected response type.</typeparam>
        /// <param name="instanceId">The instance to act upon.</param>
        /// <returns>Returns information about the specified action and instance.</returns>
        protected T setLevelPlayback<T>(int instanceId) where T : new()
        {
            T response = postResponse<T>(null, ParameterType.Cookie, instanceId.ToString());

            if (response == null)
            {
                throw new ArgumentException("Unable to retrieve response \""
                + typeof(T).ToString() + 
                "\" with instanceId \"" + instanceId.ToString() + "\".");
            }

            return response;
        }
    }
}
