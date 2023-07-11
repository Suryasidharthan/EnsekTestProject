using FluentAssertions.Equivalency;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekTestProject.EndPoints
{
    internal class Reset
    {
        private RestClient client;
        private RestRequest request;
        private RestResponse response;
        public Reset(string baseUrl)
        {
            client = new RestClient(baseUrl);
        }

        public bool ResetTestData(string accessToken)
        {
            request = new RestRequest($"/ENSEK/reset", Method.Post);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return false;
            }
            return true;
        }

    }
}
