using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsekTestProject.Models;
using Newtonsoft.Json;

namespace EnsekTestProject.EndPoints
{
    public class EnsekOrders
    {
        private RestClient client;
        private RestRequest request;
        private RestResponse response;
        

        public EnsekOrders(string baseUrl)
        {
            client = new RestClient(baseUrl);

        }
        public List<Order> GetListOfOrders(string accessToken)
        {
            request = new RestRequest($"/ENSEK/orders", Method.Get);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            var response = client.Execute(request).Content;
            List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(response);
            return orders;
        }

    }
}
