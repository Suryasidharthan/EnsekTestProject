using Newtonsoft.Json.Linq;
using RestSharp;


namespace EnsekTestProject.EndPoints
{
    internal class Login
    {
        private string _username;
        private string _password;
        private string _accessToken;
        private string _baseUrl;
        private RestClient _client;
        private RestRequest _request;
        private RestResponse _response;

        public Login(string baseUrl,string username, string password)
        { 
           this._username = username;
           this._password = password;
           this._baseUrl = baseUrl;
        }

        private void LoginToGetAccessToken()
        {
            _client = new RestClient(_baseUrl);
            _request = new RestRequest("ENSEK/login", Method.Post);
            var loginBody = new {username = this._username, password = this._password };
            _request.AddBody(loginBody);
            _response = _client.Execute(_request);
            if (_response.StatusCode!= System.Net.HttpStatusCode.OK)
            { 
                throw new Exception($"Login not successful.{_response.ErrorMessage}"); 
            }
            else 
            {
                _accessToken = grabAccessToken(_response);
            }
                
        }
        private string grabAccessToken(RestResponse response) 
        {
            JObject responseContent = JObject.Parse(response.Content);
            return responseContent["access_token"].ToString();
           
        }
        public string GetAccessToken()
        {
            if (_accessToken == null)
            {
                LoginToGetAccessToken();

            }
            return _accessToken;
        }
    }
}
