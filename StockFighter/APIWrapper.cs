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
    public class APIWrapper
    {
        private const string URL = @"https://api.stockfighter.io/ob/api/";
        private const string apiKey = @"b5b8b7f29d5aa969da22279262c1e68ff82515c4";
        private const string jsonMedia = @"application/json";

        #region API Calls
        public static bool Heartbeat()
        {
            var client = getClient();

            var request = new RestRequest(GetCommand(Command.HeartBeat));

            var response = client.Execute<HeartbeatResponse>(request);

            var heartbeatResponse = response.Data;

            if (heartbeatResponse != null)
            {
                return heartbeatResponse.ok;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckVenue(string venue)
        {
            var client = getClient();

            var request = new RestRequest(String.Format(GetCommand(Command.CheckVenue), venue));

            var response = client.Execute<VenueHeartbeatResponse>(request);

            var venueResp = response.Data;

            if (venueResp != null)
            {
                return venueResp.ok;
            }
            else
            {
                return false;
            }
        }

        #endregion
        #region Privates

        private static RestClient getClient()
        {
            var client = new RestClient(URL);
            client.Authenticator = new JwtAuthenticator(apiKey);
            return client;
        }

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
                default:
                    throw new NotImplementedException();
            }

            return cmdString;
        }

        private static bool IsSuccessful(IRestResponse response)
        {
            return false;
        }


        #endregion
    }

    public class HeartbeatResponse
    {
        public bool ok { get; set; }
        public string error { get; set; }
    }

    public class VenueHeartbeatResponse
    {
        public bool ok { get; set; }
        public string venue { get; set; }
        public string error { get; set; }
    }

    public enum Command
    {
        HeartBeat,
        CheckVenue
    }

}
