using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;
using RestSharp.Authenticators;
using System.Net.Http;
using System.Configuration;
using StockFighter.Responses;
using StockFighter.Requests;

namespace StockFighter
{
    /// <summary>
    /// Wraps Stockfighter API functions.
    /// </summary>
    public class APIWrapper
    {
        /// <summary>
        /// API URL for interfacing.
        /// </summary>
        private const string URL = @"https://api.stockfighter.io/ob/api/";

        private Dictionary<Type, string> commandDictionary;


        /// <summary>
        /// Initializes the APIWrapper.
        /// </summary>
        public APIWrapper()
        {
            commandDictionary = getCommandDictionary();
        }

        #region API Calls

        /// <summary>
        /// Checks if the server is online.
        /// </summary>
        /// <returns>Returns true if the server is up; else, false.</returns>
        public bool Heartbeat()
        {
            var heartbeatResponse = getResponse<Heartbeat>(new string[] { });

            return heartbeatResponse != null ? heartbeatResponse.ok : false;
        }

        /// <summary>
        /// Checks if a venue is available.
        /// </summary>
        /// <param name="venue">The venue to check if available.</param>
        /// <returns>Returns true if the venue is available; else, false.</returns>
        public bool CheckVenue(string venue)
        {
            var venueResponse = getResponse<VenueHeartbeat>(new string[] { venue });

            return venueResponse != null ? venueResponse.ok : false;
        }

        /// <summary>
        /// Provides a list of stocks for the supplied venue.
        /// </summary>
        /// <param name="venue">The venue to find stocks for.</param>
        /// <returns>Returns a VenueStocks response with an array of VenueStock.</returns>
        public VenueStocks GetStocks(string venue)
        {
            var getStocksResponse = getResponse<VenueStocks>(new string[] { venue });

            if(getStocksResponse != null)
            {
                return getStocksResponse;
            }

            else
            {
                throw new ArgumentException("Venue \"" + venue + "\" does not exist.");
            }
        }

        /// <summary>
        /// Gets the Orderbook for a particular stock.
        /// </summary>
        /// <param name="venue">The venue for a particular stock.</param>
        /// <param name="stock">The stock to obtain an orderbook for.</param>
        /// <returns>Returns an OrderBook response with an array of asks and bids.</returns>
        public Orderbook GetOrderbook(string venue, string stock)
        {
            var orderbook = getResponse<Orderbook>(new string[] { venue, stock });

            if(orderbook != null)
            { 
                orderbook.asks = orderbook.asks ?? new List<Orderbook.Request>();

                orderbook.bids = orderbook.bids ?? new List<Orderbook.Request>();

                return orderbook;
            }

            else
            {
                throw new ArgumentException("Stock \"" + stock + "\" in venue \""
                    + venue + "\" does not exist.");
            }
        }

        /// <summary>
        /// Gets a Quote for a specific stock in a venue.
        /// </summary>
        /// <param name="venue">The venue for a particular stock.</param>
        /// <param name="stock">The stock to obtain a quote for.</param>
        /// <returns>Returns a Quote response.</returns>
        public Quote GetQuote(string venue, string stock)
        {
            var quote = getResponse<Quote>(new string[] { venue, stock });

            if (quote != null)
            {
                return quote;
            }

            else
            {
                throw new ArgumentException("Stock \"" + stock + "\" in venue \""
                    + venue + "\" does not exist.");
            }
        }

        /// <summary>
        /// Posts an Order to the stock exchange.
        /// </summary>
        /// <param name="order">The order to post.</param>
        /// <returns>The <c ref="OrderResponse"/> for the request.</returns>
        public OrderResponse PostOrder(OrderRequest order)
        {
            var orderRequest = new _orderRequest(order);

            var orderResponse = postResponse<_orderResponse>(
                orderRequest,
                new string[] { orderRequest.venue, orderRequest.stock });

            if(orderResponse != null)
            {
                var clientOrderResponse = new OrderResponse(orderResponse);

                return clientOrderResponse;
            }

            else
            {
                throw new ArgumentException("Invalid OrderRequest.");
            }
        }

        #endregion
        #region Privates

        /// <summary>
        /// Assembles a client with the default API key.
        /// </summary>
        /// <returns>Returns an instantiated, authorized RestClient.</returns>
        private RestClient getClient()
        {
            var client = new RestClient(URL);
            return client;
        }

        /// <summary>
        /// Initializes a dictionary of API commands and returns it.
        /// </summary>
        /// <returns>Returns an initialized dictionary of API commands.</returns>
        private Dictionary<Type, string> getCommandDictionary()
        {
            var dict = new Dictionary<Type, string>
            {
                { typeof(Heartbeat), "heartbeat" },
                { typeof(VenueHeartbeat), "venues/{0}/heartbeat" },
                { typeof(VenueStocks), "venues/{0}/stocks" },
                { typeof(Orderbook), "venues/{0}/stocks/{1}" },
                { typeof(Quote), "venues/{0}/stocks/{1}/quote" },
                { typeof(_orderResponse), "venues/{0}/stocks/{1}/orders" }
            };

            return dict;
        }

        /// <summary>
        /// Posts the APIPost object and returns the response.
        /// </summary>
        /// <typeparam name="T">The type of response to return.</typeparam>
        /// <param name="post">The JSON object to post.</param>
        /// <param name="args">The parameters for the REST command.</param>
        /// <returns>Returns the response as T or null if invalid.</returns>
        private T postResponse<T>(APIRequest post, params string[] args) where T : new()
        {
            var apiKey = getApiKey();
            return performCommand<T>(post, Method.POST, apiKey, args);
        }

        /// <summary>
        /// Executes a command and returns an expected response.
        /// </summary>
        /// <typeparam name="T">The response to return.</typeparam>
        /// <param name="args">REST parameters, if needed.</param>
        /// <returns>Returns a response in the form of T or null if invalid.</returns>
        private T getResponse<T>(params string[] args) where T : new()
        {
            var apiKey = getApiKey();
            return performCommand<T>(null, Method.GET, apiKey, args);
        }

        private string getApiKey()
        {
            return ConfigurationManager.AppSettings["apiKey"];
        }

        /// <summary>
        /// Performs the specified method using specified arguments and returns the specified response.
        /// </summary>
        /// <typeparam name="T">The type of response to return.</typeparam>
        /// <param name="post">The POST object to serialize for the request, if needed.</param>
        /// <param name="method">The method to utilize the REST service with.</param>
        /// <param name="apiKey">The authorizing API key.</param>
        /// <param name="args">REST parameters, if needed.</param>
        /// <returns>Returns a response in the form of T or null if invalid.</returns>
        private T performCommand<T>(APIRequest post, Method method, string apiKey, string[] args) where T : new()
        {
            var authorizationParameter = @"X-Starfighter-Authorization";

            var client = getClient();

            var rawCommandString = getCommand(typeof(T));

            var commandString = String.Format(rawCommandString, args);

            var request = new RestRequest(commandString, method);

            if (post != null)
            {
                request.RequestFormat = DataFormat.Json;

                request.AddParameter(authorizationParameter, apiKey, ParameterType.HttpHeader);

                request.AddBody(post);
            }

            var response = client.Execute<T>(request);

            return response.Data;
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

        #endregion
    }
}
