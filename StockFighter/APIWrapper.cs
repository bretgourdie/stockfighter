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
        /// Posts the APIPost object and returns the response.
        /// </summary>
        /// <typeparam name="T">The type of response to return.</typeparam>
        /// <param name="post">The JSON object to post.</param>
        /// <param name="args">The parameters for the REST command.</param>
        /// <returns>Returns the response as T or null if invalid.</returns>
        private T postResponse<T>(APIPost post, params string[] args) where T : new()
        {
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
            return performCommand<T>(null, Method.GET, apiKey, args);
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
        private T performCommand<T>(APIPost post, Method method, string apiKey, string[] args) where T : new()
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
        /// <typeparam name="T">A deserialized <c ref="APIResponse"/> class.</typeparam>
        /// <returns>
        /// Returns a command string with the parameters 
        /// ready to be replaced by String.Format.
        /// </returns>
        private string getCommand(Type type)
        {
            var switchDict = new Dictionary<Type, string>
            {
                { typeof(Heartbeat), "heartbeat" },
                { typeof(VenueHeartbeat), "venues/{0}/heartbeat" },
                { typeof(VenueStocks), "venues/{0}/stocks" },
                { typeof(Orderbook), "venues/{0}/stocks/{1}" },
                { typeof(Quote), "venues/{0}/stocks/{1}/quote" },
                { typeof(_orderResponse), "venues/{0}/stocks/{1}/orders" }
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
        private bool IsSuccessful(IRestResponse response)
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
    /// Internal representation of an OrderResponse.
    /// </summary>
    internal class _orderResponse : AbstractOrderResponse
    {
        /// <summary>
        /// A stringified version of the order's direction.
        /// </summary>
        public string direction { get; set; }
        /// <summary>
        /// A stringified version of the order's type.
        /// </summary>
        public string orderType { get; set; }
    }

    /// <summary>
    /// A client-facing version of an OrderResponse.
    /// </summary>
    public class OrderResponse : AbstractOrderResponse
    {
        /// <summary>
        /// The order's direction.
        /// </summary>
        public OrderDirection direction { get; set; }
        /// <summary>
        /// The order's type.
        /// </summary>
        public OrderType orderType { get; set; }

        /// <summary>
        /// Creates a client-facing OrderResponse.
        /// </summary>
        /// <param name="orderResponse">
        /// The deserialized order response from the APIWrapper.
        /// </param>
        internal OrderResponse(_orderResponse orderResponse)
        {
            this.ok = orderResponse.ok;
            this.symbol = orderResponse.symbol;
            this.venue = orderResponse.venue;
            this.direction = getDirection(orderResponse.direction);
            this.originalQty = orderResponse.originalQty;
            this.qty = orderResponse.qty;
            this.price = orderResponse.price;
            this.orderType = getType(orderResponse.orderType);
            this.id = orderResponse.id;
            this.account = orderResponse.account;
            this.ts = orderResponse.ts;
            this.fills = orderResponse.fills;
            this.totalFilled = orderResponse.totalFilled;
            this.open = orderResponse.open;
        }

        /// <summary>
        /// Translates the stringified order direction to an OrderDirection.
        /// </summary>
        /// <param name="direction">The direction of the order.</param>
        /// <returns>
        /// Returns the OrderDirection of the order direction.
        /// </returns>
        private OrderDirection getDirection(string direction)
        {
            OrderDirection eDirection;

            switch (direction)
            {
                case "buy":
                    eDirection = OrderDirection.Buy;
                    break;
                case "sell":
                    eDirection = OrderDirection.Sell;
                    break;
                default:
                    throw new NotImplementedException("Direction \""
                        + direction + "\" was not implemented.");
            }

            return eDirection;
        }

        /// <summary>
        /// Translates the stringified order type to an OrderType.
        /// </summary>
        /// <param name="type">The type of the order.</param>
        /// <returns>Returns the OrderType of the order type.</returns>
        private OrderType getType(string type)
        {
            OrderType eType;

            switch (type)
            {
                case "limit":
                    eType = OrderType.Limit;
                    break;
                case "market":
                    eType = OrderType.Market;
                    break;
                case "fill-or-kill":
                    eType = OrderType.FillOrKill;
                    break;
                case "immediate-or-cancel":
                    eType = OrderType.ImmediateOrCancel;
                    break;
                default:
                    throw new NotImplementedException("Type \""
                        + type + "\" was not implemented.");
            }

            return eType;
        }
    }

    /// <summary>
    /// Base for all types of OrderResponse.
    /// </summary>
    public abstract class AbstractOrderResponse : APIResponse
    {
        /// <summary>
        /// The stock's symbol.
        /// </summary>
        public string symbol { get; set; }
        /// <summary>
        /// The stock's venue.
        /// </summary>
        public string venue { get; set; }
        /// <summary>
        /// The original quantity of the order.
        /// </summary>
        public int originalQty { get; set; }
        /// <summary>
        /// The quantity left outstanding from executed fills.
        /// </summary>
        public int qty { get; set; }
        /// <summary>
        /// The price of the order's fill. May not match the fills!
        /// </summary>
        public int price { get; set; }
        /// <summary>
        /// A guaranteed unique number on this venue.
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// The account used to execute the trade.
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// The time the order was received.
        /// </summary>
        public DateTime ts { get; set; }
        /// <summary>
        /// A list of fills for the order.
        /// </summary>
        public List<Fill> fills { get; set; }
        /// <summary>
        /// The amount of quantity that was filled.
        /// </summary>
        public int totalFilled { get; set; }
        /// <summary>
        /// If the order can still be filled.
        /// </summary>
        public bool open { get; set; }

        /// <summary>
        /// Don't allow AbstractOrderResponse to be inherited from 
        /// outside of the assembly.
        /// </summary>
        internal AbstractOrderResponse() { }
    }

    /// <summary>
    /// Representation of a fill from an OrderResponse.
    /// </summary>
    public class Fill
    {
        /// <summary>
        /// The price the order was filled at.
        /// </summary>
        public int price { get; set; }
        /// <summary>
        /// The amount filled.
        /// </summary>
        public int qty { get; set; }
        /// <summary>
        /// The time the fill occurred.
        /// </summary>
        public DateTime ts { get; set; }
    }


    /// <summary>
    /// Deserialized representation of an Order post.
    /// </summary>
    internal class _orderRequest : AbstractOrderRequest 
    {
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

        /// <summary>
        /// Translates a client's OrderRequest to an API-ready _orderRequest.
        /// </summary>
        /// <param name="orderRequest">The client's order request.</param>
        public _orderRequest(OrderRequest orderRequest)
        {
            this.account = orderRequest.account;
            this.venue = orderRequest.venue;
            this.stock = orderRequest.stock;
            this.price = orderRequest.price;
            this.qty = orderRequest.qty;
            this.orderType = getOrderType(orderRequest.ordertype);
            this.direction = getDirection(orderRequest.direction);
        }

        /// <summary>
        /// Returns stringified version of the supplied OrderDirection.
        /// </summary>
        /// <param name="direction">The direction of the order.</param>
        /// <returns>
        /// Returns a stringified OrderDirection for the API.
        /// </returns>
        private string getDirection(OrderDirection direction)
        {
            string sDirection = "";
            switch (direction)
            {
                case OrderDirection.Buy:
                    sDirection = "buy";
                    break;
                case OrderDirection.Sell:
                    sDirection = "sell";
                    break;
                default:
                    throw new NotImplementedException("Direction \""
                        + direction.ToString() + "\" not implemented.");
            }

            return sDirection;
        }

        /// <summary>
        /// Returns stringified version of the supplied OrderType.
        /// </summary>
        /// <param name="type">The type of the order.</param>
        /// <returns>A stringified OrderType for the API.</returns>
        private string getOrderType(OrderType type)
        {
            string sType = "";
            switch (type)
            {
                case OrderType.Limit:
                    sType = "limit";
                    break;
                case OrderType.Market:
                    sType = "market";
                    break;
                case OrderType.FillOrKill:
                    sType = "fill-or-kill";
                    break;
                case OrderType.ImmediateOrCancel:
                    sType = "immediate-or-cancel";
                    break;
                default:
                    throw new NotImplementedException("Type \""
                        + type.ToString() + "\" not implemented.");
            }

            return sType;
        }
    }

    /// <summary>
    /// User-facing representation of an Order post.
    /// </summary>
    public class OrderRequest : AbstractOrderRequest
    {
        /// <summary>
        /// The type of the order request.
        /// </summary>
        public OrderType ordertype { get; set; }
        /// <summary>
        /// The direction of the order request.
        /// </summary>
        public OrderDirection direction { get; set; }

        /// <summary>
        /// Creates an OrderRequest.
        /// Use the APIWrapper to post it.
        /// </summary>
        /// <param name="account">
        /// The account to purchase the order with.
        /// </param>
        /// <param name="venue">The venue of the stock.</param>
        /// <param name="stock">The stock's symbol.</param>
        /// <param name="price">The price to buy/sell at.</param>
        /// <param name="qty">The amount of stock to buy/sell.</param>
        /// <param name="direction">
        /// The decision to buy or sell the stock.
        /// </param>
        /// <param name="ordertype">
        /// The type of order to put.
        /// </param>
        public OrderRequest(
            string account,
            string venue,
            string stock,
            int price,
            int qty,
            OrderDirection direction,
            OrderType ordertype)
        {
            this.account = account;
            this.venue = venue;
            this.stock = stock;
            this.price = price;
            this.qty = qty;
            this.direction = direction;
            this.ordertype = ordertype;
        }
    }

    /// <summary>
    /// Direction of order.
    /// </summary>
    public enum OrderDirection
    {
        /// <summary>
        /// Desire to bid.
        /// </summary>
        Buy,
        /// <summary>
        /// Desire to ask.
        /// </summary>
        Sell
    }

    /// <summary>
    /// Type of order.
    /// </summary>
    public enum OrderType 
    {
        /// <summary>
        /// <para>
        /// The most common order.
        /// </para>
        /// <para>
        /// Immediately matches any orders offering prices "as good or better
        /// as the ones listed in the order."
        /// Has any unmatched portion of the order rests on the orderbook.
        /// </para>
        /// <para>
        /// Good until cancelled.
        /// </para>
        /// </summary>
        Limit,
        /// <summary>
        /// Executes immediately regardless of price.
        /// </summary>
        Market,
        /// <summary>
        /// Limit order for immediate execution on an all-or-nothing basis.
        /// The order may be accepted ("ok":true) but not receive any fills
        /// ("open":false).
        /// </summary>
        FillOrKill,
        /// <summary>
        /// A <c ref="OrderType.FillOrKill"/> that accepts partial execution.
        /// </summary>
        ImmediateOrCancel
    }

    public abstract class AbstractOrderRequest : APIPost
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
        public virtual int qty { get; set; }
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
