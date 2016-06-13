using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;
using StockFighter.Requests;
using System.Configuration;

namespace StockFighter
{
    /// <summary>
    /// Abstract API wrapper. Does the heavy lifting.
    /// </summary>
    public abstract class AbstractAPI
    {
        /// <summary>
        /// API URL for interfacing with commands.
        /// </summary>
        protected abstract string url { get; }

        /// <summary>
        /// The lookup for a command for an expected response.
        /// 
        /// Define translations in <c ref="getCommandDictionary()"/>.
        /// </summary>
        protected Dictionary<Type, string> commandDictionary;

        /// <summary>
        /// The internal API Key.
        /// </summary>
        private string _apiKey;

        /// <summary>
        /// The API Key to use for the API instance.
        /// </summary>
        public string APIKey
        {
            get
            {
                return _apiKey;
            }
        }

        /// <summary>
        /// Initializes an API with the default API Key in App.config.
        /// </summary>
        public AbstractAPI() : this(AbstractAPI.getDefaultAPIKey()) { }

        /// <summary>
        /// Initializes an API with the specified API Key.
        /// </summary>
        /// <param name="apiKey">The API Key to authorize with.</param>
        public AbstractAPI(string apiKey)
        {
            commandDictionary = getCommandDictionary();
            _apiKey = apiKey;
        }

        /// <summary>
        /// Initializes a dictionary of API commands and returns it.
        /// </summary>
        /// <returns>Returns an initialized dictionary of API commands.</returns>
        protected abstract Dictionary<Type, string> getCommandDictionary();

        /// <summary>
        /// Gets the default API Key: the "apiKey" value from App.config.
        /// </summary>
        /// <returns>Returns the default API Key.</returns>
        protected static string getDefaultAPIKey()
        {
            return ConfigurationManager.AppSettings["apiKey"];
        }

        /// <summary>
        /// Assembles a client with the default API key.
        /// </summary>
        /// <returns>Returns an instantiated, authorized RestClient.</returns>
        protected RestClient getClient()
        {
            var client = new RestClient(url);
            return client;
        }

        /// <summary>
        /// Posts the APIPost object and returns the response.
        /// </summary>
        /// <typeparam name="T">The type of response to return.</typeparam>
        /// <param name="post">The JSON object to post.</param>
        /// <param name="authParameterType">The type of parameter to use for authentication.</param>
        /// <param name="args">The parameters for the REST command.</param>
        /// <returns>Returns the response as T or null if invalid.</returns>
        protected T postResponse<T>(
            APIRequest post, 
            ParameterType authParameterType, 
            params string[] args) 
            where T : new()
        {
            return performCommand<T>(post, Method.POST, authParameterType, this.APIKey, args);
        }

        /// <summary>
        /// Executes a command and returns an expected response.
        /// </summary>
        /// <typeparam name="T">The response to return.</typeparam>
        /// <param name="args">REST parameters, if needed.</param>
        /// <returns>Returns a response in the form of T or null if invalid.</returns>
        protected T getResponse<T>(params string[] args) where T : new()
        {
            return performCommand<T>(null, Method.GET, ParameterType.HttpHeader, this.APIKey, args);
        }

        /// <summary>
        /// Performs the specified method using specified arguments and returns the specified response.
        /// </summary>
        /// <typeparam name="T">The type of response to return.</typeparam>
        /// <param name="post">The POST object to serialize for the request, if needed.</param>
        /// <param name="method">The method to utilize the REST service with.</param>
        /// <param name="authParameterType">The type of parameter to use for authentication.</param>
        /// <param name="apiKey">The authorizing API key.</param>
        /// <param name="args">REST parameters, if needed.</param>
        /// <returns>Returns a response in the form of T or null if invalid.</returns>
        private T performCommand<T>(
            APIRequest post, 
            Method method, 
            ParameterType authParameterType, 
            string apiKey, 
            string[] args) 
            where T : new()
        {
            var authorizationParameter = @"X-Starfighter-Authorization";

            var client = getClient();

            var rawCommandString = getCommand(typeof(T));

            var commandString = String.Format(rawCommandString, args);

            var request = new RestRequest(commandString, method);

            request = setRequestFormat(request, DataFormat.Json);

            request = authorizeRequest(request, authorizationParameter, apiKey, authParameterType);

            if (post != null)
            {
                request.AddBody(post);
            }

            var response = client.Execute<T>(request);

            return response.Data;
        }

        /// <summary>
        /// Sets the request's RequestFormat to the specified DataFormat.
        /// </summary>
        /// <param name="request">The request to set RequestFormat of.</param>
        /// <param name="requestFormat">The target RequestFormat.</param>
        /// <returns>Returns a RestRequest with the specified RequestFormat.</returns>
        protected RestRequest setRequestFormat(
            RestRequest request,
            DataFormat requestFormat)
        {
            request.RequestFormat = requestFormat;

            return request;
        }

        /// <summary>
        /// Authorizes the RestRequest using the Key-Value pair and the supplied ParameterType.
        /// </summary>
        /// <param name="request">The request to authorize.</param>
        /// <param name="authorizationKey">The "Key" of the Key-Value authorization pair.</param>
        /// <param name="authorizationValue">The "Value" of the Key-Value authorization pair.</param>
        /// <param name="authParameterType">The ParameterType to authorize as.</param>
        /// <returns>Returns an authorized RestRequest.</returns>
        protected RestRequest authorizeRequest(
            RestRequest request,
            string authorizationKey,
            string authorizationValue,
            ParameterType authParameterType)
        {
            request.AddParameter(authorizationKey, authorizationValue, authParameterType);

            return request;
        }

        /// <summary>
        /// Translation matrix for a deserialized response to the URL.
        /// </summary>
        /// <remarks>
        /// Any REST parameters will appear in the returned string as format hooks.
        /// </remarks>
        /// <returns>
        /// Returns a command string with the parameters 
        /// ready to be replaced by String.Format.
        /// </returns>
        private string getCommand(Type type)
        {
            if (commandDictionary.Keys.Contains(type))
            {
                return commandDictionary[type];
            }

            else
            {
                throw new NotImplementedException(
                    "Class \"" + type.ToString() + "\" has not been implemented.");
            }
        }

        /// <summary>
        /// Checks if the response was successful.
        /// </summary>
        /// <param name="response">The response received from the request.</param>
        /// <returns>Returns true if the request was completed; else, false.</returns>
        private bool IsSuccessful(IRestResponse response)
        {
            return response.ResponseStatus == ResponseStatus.Completed;
        }
    }
}
