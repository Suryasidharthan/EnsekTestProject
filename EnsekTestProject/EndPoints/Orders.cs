using RestSharp;
using EnsekTestProject.Models;
using Newtonsoft.Json;

namespace EnsekTestProject.EndPoints
{
    internal class Orders
    {
        private RestClient _client;
        private RestRequest _request;
        
        public Orders(string baseUrl)
        {
            _client = new RestClient(baseUrl);

        }
        public List<Order> GetListOfOrders(string accessToken)
        {
            _request = new RestRequest($"/ENSEK/orders", Method.Get);
            _request.AddHeader("Authorization", "Bearer " + accessToken);
            var response = _client.Execute(_request).Content;
            List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(response);
            return orders;
        }

    }
}
