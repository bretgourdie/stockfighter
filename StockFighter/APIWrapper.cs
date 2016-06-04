using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;
using RestSharp.Authenticators;
using System.Net.Http;

namespace StockFighter
{
    /// <summary>
    /// Static class to wrap Stockfighter API functions
    /// </summary>
    public class APIWrapper
    {
        /// <summary>
        /// API URL for interfacing.
        /// </summary>
        private const string URL = @"https://api.stockfighter.io/ob/api/";
        /// <summary>
        /// Default API Key for authorization.
        /// </summary>
        private const string apiKey = @"b5b8b7f29d5aa969da22279262c1e68ff82515c4";
        /// <summary>
        /// Specification of JSON media type.
        /// </summary>
        private const string jsonMedia = @"application/json";

        #region API Calls

        /// <summary>
        /// Checks if the server is online.
        /// </summary>
        /// <returns>Returns true if the server is up; else, false.</returns>
        public static bool Heartbeat()
        {
            var heartbeatResponse = GetResponse<Heartbeat>(new string[] { });

            return heartbeatResponse != null ? heartbeatResponse.ok : false;
        }

        /// <summary>
        /// Checks if a venue is available.
        /// </summary>
        /// <param name="venue">The venue to check if available.</param>
        /// <returns>Returns true if the venue is available; else, false.</returns>
        public static bool CheckVenue(string venue)
        {
            var venueResponse = GetResponse<VenueHeartbeat>(new string[] { venue });

            return venueResponse != null ? venueResponse.ok : false;
        }

        /// <summary>
        /// Provides a list of stocks for the supplied venue.
        /// </summary>
        /// <param name="venue">The venue to find stocks for.</param>
        /// <returns>Returns a VenueStocks response with an array of VenueStock.</returns>
        public static VenueStocks GetStocks(string venue)
        {
            var getStocksResponse = GetResponse<VenueStocks>(new string[] { venue });

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
        /// <returns>Returns an OrderBook reponse with an array of asks and bids.</returns>
        public static Orderbook GetOrderbook(string venue, string stock)
        {
            var orderbook = GetResponse<Orderbook>(new string[] { venue, stock });

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

        public static Quote GetQuote(string venue, string stock)
        {
            var quote = GetResponse<Quote>(new string[] { venue, stock });

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

        #endregion
        #region Privates

        /// <summary>
        /// Assembles a client with the default API key.
        /// </summary>
        /// <returns>Returns an instantiated, authorized RestClient.</returns>
        private static RestClient getClient()
        {
            var client = new RestClient(URL);
            client.Authenticator = new JwtAuthenticator(apiKey);
            return client;
        }

        private static T GetResponse<T>(string[] args) where T : new()
        {
            var client = getClient();

            var rawCommandString = getCommand(typeof(T));

            var commandString = String.Format(rawCommandString, args);

            var request = new RestRequest(commandString);

            var response = client.Execute<T>(request);

            return response.Data;
        }

        /// <summary>
        /// Translation matrix for a deserialized response to the URL.
        /// </summary>
        /// <remarks>
        /// Any REST parameters will appear in the returned string as format hooks.
        /// </remarks>
        /// <typeparam name="T">A deserialized <c ref="APIResponse"/> class.</typeparam>
        /// <returns>
        /// Returns a command string with the parameters 
        /// ready to be replaced by String.Format.
        /// </returns>
        private static string getCommand(Type type)
        {
            var switchDict = new Dictionary<Type, string>
            {
                { typeof(Heartbeat), "heartbeat" },
                { typeof(VenueHeartbeat), "venues/{0}/heartbeat" },
                { typeof(VenueStocks), "venues/{0}/stocks" },
                { typeof(Orderbook), "venues/{0}/stocks/{1}" },
                { typeof(Quote), "venues/{0}/stocks/{1}/quote" }
            };

            var dict = new Dictionary<Type, string>();

            if (switchDict.Keys.Contains(type))
            {
                return switchDict[type];
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
        private static bool IsSuccessful(IRestResponse response)
        {
            return response.ResponseStatus == ResponseStatus.Completed;
        }


        #endregion
    }

    /// <summary>
    /// Deserialized response from a Heartbeat command.
    /// </summary>
    public class Heartbeat : APIResponse { }

    /// <summary>
    /// Deserialized response from a CheckVenue command.
    /// </summary>
    public class VenueHeartbeat : APIResponse
    {
        /// <summary>
        /// Venue that heartbeat was queried for.
        /// </summary>
        public string venue { get; set; }
    }

    /// <summary>
    /// Deserialized response from a GetStocks command.
    /// </summary>
    public class VenueStocks : APIResponse
    {
        /// <summary>
        /// List of stocks in the venue.
        /// </summary>
        public List<VenueStock> symbols { get; set; }
    }

    /// <summary>
    /// Deserialized stocks from a GetStocks command.
    /// </summary>
    public class VenueStock : APIResponse
    {
        /// <summary>
        /// The name of the stock.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The symbol of the stock.
        /// </summary>
        public string symbol { get; set; }
    }

    /// <summary>
    /// Deserialized response from a <c ref="GetOrderbook"/> command.
    /// </summary>
    public class Orderbook : APIResponse
    {
        /// <summary>
        /// The venue of the stock.
        /// </summary>
        public string venue { get; set; }
        /// <summary>
        /// The stock's symbol.
        /// </summary>
        public string symbol { get; set; }
        /// <summary>
        /// The list of bids (buys) for the stock.
        /// </summary>
        public List<Orderbook.Request> bids { get; set; }
        /// <summary>
        /// The list of asks (sells) for the stock.
        /// </summary>
        public List<Orderbook.Request> asks { get; set; }
        /// <summary>
        /// The timestamp the orderbook was retrieved.
        /// </summary>
        public DateTime ts { get; set; }

        /// <summary>
        /// Deserialized representation of an orderbook request (bid or ask).
        /// </summary>
        public class Request
        {
            /// <summary>
            /// The price of the request.
            /// </summary>
            public decimal price { get; set; }
            /// <summary>
            /// The quantity of the request.
            /// </summary>
            public int qty { get; set; }
            /// <summary>
            /// Is this a request to buy (bid) or sell (ask)?
            /// </summary>
            public bool isBuy { get; set; }
        }
    }

    /// <summary>
    /// Deserialized representation of a quote request.
    /// </summary>
    public class Quote : APIResponse
    {
        /// <summary>
        /// The stocks's symbol.
        /// </summary>
        public string symbol { get; set; }
        /// <summary>
        /// The venue of the stock.
        /// </summary>
        public string venue { get; set; }
        /// <summary>
        /// The best price currently bid for the stock.
        /// </summary>
        public int bid { get; set; }
        /// <summary>
        /// The best price currently offered for the stock.
        /// </summary>
        public int ask { get; set; }
        /// <summary>
        /// The aggregate size of all orders at the best bid.
        /// </summary>
        public int bidSize { get; set; }
        /// <summary>
        /// The aggregate size of all orders at the best ask.
        /// </summary>
        public int askSize { get; set; }
        /// <summary>
        /// The aggregate size of all bids.
        /// </summary>
        public int bidDepth { get; set; }
        /// <summary>
        /// The aggregate size of all asks.
        /// </summary>
        public int askDepth { get; set; }
        /// <summary>
        /// The price of the last trade.
        /// </summary>
        public int last { get; set; }
        /// <summary>
        /// The quantity of the last trade.
        /// </summary>
        public int lastSize { get; set; }
        /// <summary>
        /// The timestamp of the last trade.
        /// </summary>
        public DateTime lastTrade { get; set; }
        /// <summary>
        /// The timestamp the quote was last updated server-side.
        /// </summary>
        public DateTime quoteTime { get; set; }
    }


    /// <summary>
    /// Deserialized representation of an Order post.
    /// </summary>
    private class _orderRequest : APIPost
    {
        /// <summary>
        /// The trading account you are trading for.
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// The venue to place the trade to.
        /// </summary>
        public string venue { get; set; }
        /// <summary>
        /// The stock symbol to place the trade in.
        /// </summary>
        public string stock { get; set; }
        /// <summary>
        /// The desired price. Ignored for market orders.
        /// </summary>
        public int price { get; set; }
        /// <summary>
        /// The desired quantity.
        /// </summary>
        public int qty { get; set; }
        /// <summary>
        /// Whether you want to buy or sell.
        /// </summary>
        /// <remarks>
        /// To bid, use "buy". To ask, use "sell".
        /// </remarks>
        public string direction { get; set; }
        /// <summary>
        /// The order type.
        /// </summary>
        public string orderType { get; set; }
    }

    /// <summary>
    /// Base class for all responses. Should be used for commonalities.
    /// </summary>
    public class APIResponse
    {
        /// <summary>
        /// Command was received correctly.
        /// </summary>
        public bool ok { get; set; }
        /// <summary>
        /// Error text. If no error, will be an empty string.
        /// </summary>
        public string error { get; set; }
    }

    /// <summary>
    /// Base class for all Post requests. Should be used for commonalities.
    /// </summary>
    public class APIPost
    {

    }
}
