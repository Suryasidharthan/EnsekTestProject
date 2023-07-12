using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using System.Text.RegularExpressions;


namespace EnsekTestProject.EndPoints
{
    internal class BuyFuel
    {
        private RestClient _client;
        private RestRequest _request;
        private RestResponse _response;
        public BuyFuel(string baseUrl)
        {
            _client = new RestClient(baseUrl);
          
        }
        public string BuyFuelWithFuelIdAndQuantity(int fuelId, int quantity, string accessToken)
        {
            string orderId;
            _request = new RestRequest($"/ENSEK/buy/{fuelId}/{quantity}", Method.Put);
            _request.AddHeader("Authorization", "Bearer " + accessToken);
            _response = _client.Execute(_request);

            if (_response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Failed to buy fuel: " + _response.ErrorMessage);
            }
            JObject orderResult = JObject.Parse(_response.Content);
            string message = orderResult["message"].ToString();  // Get the message displayed in body for this request
            string pattern = @"\b[\da-fA-F]{8}-[\da-fA-F]{4}-[\da-fA-F]{4}-[\da-fA-F]{4}-[\da-fA-F]{12}\b"; //Regular expression to fetch the order UUID from the message.
            Match match = Regex.Match(message, pattern);
            if (match.Success)
            {
                orderId = match.Value;
                
            }
            else throw new Exception("No match found for OrderId in the message."); //throw if order not present in the orders list.

            return orderId;
        }
    }
}
