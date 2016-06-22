using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;
using RestSharp.Authenticators;
using System.Net.Http;
using System.Configuration;
using StockFighter.Common;
using StockFighter.API.Responses;
using StockFighter.API.Requests;

namespace StockFighter.API
{
    /// <summary>
    /// Wraps Stockfighter API functions.
    /// </summary>
    public class StockFighterAPI : AbstractAPI
    {
        /// <summary>
        /// Creates a StockFighterAPI object with the specified API Key.
        /// </summary>
        /// <param name="apiKey">The API key to authorize with.</param>
        public StockFighterAPI(string apiKey)
            : base(apiKey) { }

        /// <summary>
        /// URL for interfacing with the StockFighter API.
        /// </summary>
        protected override string url { get { return @"https://api.stockfighter.io/ob/api"; } }

        /// <summary>
        /// Authorization parameter name for StockFighter API calls.
        /// </summary>
        protected override string authorizationParameterName { get { return @"X-Starfighter-Authorization"; } }

        /// <summary>
        /// Initializes a dictionary of StockFighter API commands and returns it.
        /// </summary>
        /// <returns>Returns an initialized dictionary of StockFighter API commands.</returns>
        protected override Dictionary<Type, string> getCommandDictionary()
        {
            var dict = new Dictionary<Type, string>
            {
                { typeof(Heartbeat), "/heartbeat" },
                { typeof(VenueHeartbeat), "/venues/{0}/heartbeat" },
                { typeof(VenueStocks), "/venues/{0}/stocks" },
                { typeof(Orderbook), "/venues/{0}/stocks/{1}" },
                { typeof(Quote), "/venues/{0}/stocks/{1}/quote" },
                { typeof(_orderResponse), "/venues/{0}/stocks/{1}/orders" },
                { typeof(_existingOrderStatus), "/venues/{0}/stocks/{1}/orders/{2}"},
                { typeof(_cancelledOrder), "/venues/{0}/stocks/{1}/orders/{2}"}
            };

            return dict;
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
                ParameterType.HttpHeader,
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

        /// <summary>
        /// Retrieve the status about an existing order.
        /// </summary>
        /// <param name="orderId">The order's ID.</param>
        /// <param name="venue">The venue for the order's stock.</param>
        /// <param name="stock">The symbol for the order's stock.</param>
        /// <returns>Returns information about the existing order.</returns>
        public ExistingOrderStatus GetOrderStatus(int orderId, string venue, string stock)
        {
            var orderStatus = getResponse<_existingOrderStatus>(
                new string[] { venue, stock, orderId.ToString() });

            if (orderStatus != null)
            {
                var clientOrderStatus = new ExistingOrderStatus(orderStatus);

                return clientOrderStatus;
            }

            else
            {
                throw new ArgumentException("Invalid Order.");
            }
        }

        /// <summary>
        /// Cancels the specified order.
        /// </summary>
        /// <param name="orderId">The order's ID.</param>
        /// <param name="venue">The venue for the order's stock.</param>
        /// <param name="stock">The symbol for the order's stock.</param>
        /// <returns>Returns information about the cancelled order.</returns>
        public CancelledOrder CancelOrder(int orderId, string venue, string stock)
        {
            var cancelledOrder = deleteResponse<_cancelledOrder>(
                new string[] { venue, stock, orderId.ToString() });

            if (cancelledOrder != null)
            {
                var clientCancelledOrder = new CancelledOrder(cancelledOrder);

                return clientCancelledOrder;
            }

            else
            {
                throw new ArgumentException("Could not cancel order.");
            }
        }

        /// <summary>
        /// Retrieves the status of all existing orders.
        /// </summary>
        /// <param name="venue">The venue for the order's stock.</param>
        /// <param name="account">The account to retrieve the orders for.</param>
        /// <returns>Returns information about all existing orders.</returns>
        public AllExistingOrderStatuses GetAllOrderStatuses(string venue, string account)
        {
            var existingOrders = getResponse<_allExistingOrderStatuses>(
                new string[] { venue, account });

            if (existingOrders != null)
            {
                var clientExistingOrders = new AllExistingOrderStatuses(existingOrders);

                return clientExistingOrders;
            }

            else
            {
                throw new ArgumentException("Could not get status of all orders.");
            }
        }

        #endregion


    }
}
