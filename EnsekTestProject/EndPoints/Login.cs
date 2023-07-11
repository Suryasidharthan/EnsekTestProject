using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace EnsekTestProject.EndPoints
{
    internal class Login
    {
        private string username;
        private string password;
        private string accessToken;
        private string baseUrl;
        private RestClient client;
        private RestRequest request;
        private RestResponse response;

        public Login(string baseUrl,string username, string password)
        { 
           this.username = username;
           this.password = password;
           this.baseUrl = baseUrl;
        }

        private void LoginToGetAccessToken()
        {
            client = new RestClient(baseUrl);
            request = new RestRequest("ENSEK/login", Method.Post);
            var loginBody = new {username = this.username, password = this.password};
            request.AddBody(loginBody);
            response = client.Execute(request);
            if (response.StatusCode!= System.Net.HttpStatusCode.OK)
            { 
                throw new Exception($"Login not successful.{response.ErrorMessage}"); 
            }
            else 
            { 
                accessToken = grabAccessToken(response);
            }
                
        }
        private string grabAccessToken(RestResponse response) 
        {
            JObject responseContent = JObject.Parse(response.Content);
            return responseContent["access_token"].ToString();
           
        }
        public string GetAccessToken()
        {
            if (accessToken == null)
            {
                LoginToGetAccessToken();

            }
            return accessToken;
        }
    }
}
