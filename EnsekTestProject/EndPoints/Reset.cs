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
        private RestClient _client;
        private RestRequest _request;
        private RestResponse _response;
        public Reset(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        public bool ResetTestData(string accessToken)
        {
            _request = new RestRequest($"/ENSEK/reset", Method.Post);
            _request.AddHeader("Authorization", "Bearer " + accessToken);
            _response = _client.Execute(_request);
            if (_response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return false;
            }
            return true;
        }

    }
}
