using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using System.Text.RegularExpressions;


namespace EnsekTestProject.EndPoints
{
    public class EnsekBuyFuel
    {
        private RestClient client;
        private RestRequest request;
        private RestResponse response;
        public EnsekBuyFuel(string baseUrl)
        {
            client = new RestClient(baseUrl);
          
        }
        public string BuyFuel(int fuelId, int quantity, string accessToken)
        {
            string orderId;
            request = new RestRequest($"/ENSEK/buy/{fuelId}/{quantity}", Method.Put);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Failed to buy fuel: " + response.ErrorMessage);
            }
            JObject orderResult = JObject.Parse(response.Content);
            string message = orderResult["message"].ToString();

            string pattern = @"\b[\da-fA-F]{8}-[\da-fA-F]{4}-[\da-fA-F]{4}-[\da-fA-F]{4}-[\da-fA-F]{12}\b";
            Match match = Regex.Match(message, pattern);
            if (match.Success)
            {
                orderId = match.Value;
                
            }
            else throw new Exception("No match found for OrderId in the given order acknowledgement.");

            return orderId;
        }
    }
}
