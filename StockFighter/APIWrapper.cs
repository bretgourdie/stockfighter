﻿using System;
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
            var client = getClient();

            var request = new RestRequest(GetCommand(Command.HeartBeat));

            var response = client.Execute<HeartbeatResponse>(request);

            if (IsSuccessful(response))
            {
                var heartbeatResponse = response.Data;

                return heartbeatResponse.ok;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a venue is available.
        /// </summary>
        /// <param name="venue">The venue to check if available.</param>
        /// <returns>Returns true if the venue is available; else, false.</returns>
        public static bool CheckVenue(string venue)
        {
            var client = getClient();

            var request = new RestRequest(String.Format(GetCommand(Command.CheckVenue), venue));

            var response = client.Execute<VenueHeartbeatResponse>(request);

            if (IsSuccessful(response))
            {
                var venueResp = response.Data;

                return venueResp.ok;
            }

            else
            {
                return false;
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

        /// <summary>
        /// Translation matrix for a command enum to the URL.
        /// </summary>
        /// <remarks>
        /// Any REST parameters will appear in the returned string as format hooks.
        /// </remarks>
        /// <param name="command">The command to retrieve the REST string for.</param>
        /// <returns>
        /// Returns a command string with the parameters 
        /// ready to be replaced by String.Format.
        /// </returns>
        private static string GetCommand(Command command)
        {
            var cmdString = "";

            switch (command)
            {
                case Command.HeartBeat:
                    cmdString = "heartbeat";
                    break;
                case Command.CheckVenue:
                    cmdString = "venues/{0}/heartbeat";
                    break;
                case Command.GetStocks:
                    cmdString = "venues/{0}/stocks";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return cmdString;
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
    public class HeartbeatResponse : APIResponse
    {
        /// <summary>
        /// Error text (if no error, then empty).
        /// </summary>
        public string error { get; set; }
    }

    /// <summary>
    /// Deserialized response from a CheckVenue command.
    /// </summary>
    public class VenueHeartbeatResponse : APIResponse
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
        public VenueStock[] symbols { get; set; }
    }

    /// <summary>
    /// Deserialized stocks from a GetStocks command.
    /// </summary>
    public class VenueStock
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
    /// API Commands.
    /// </summary>
    public enum Command
    {
        /// <summary>
        /// Checks if the server is up.
        /// </summary>
        HeartBeat,
        /// <summary>
        /// Checks if a venue is up.
        /// </summary>
        CheckVenue,
        /// <summary>
        /// Gets a list of stocks on a particular venue.
        /// </summary>
        GetStocks
    }

}
