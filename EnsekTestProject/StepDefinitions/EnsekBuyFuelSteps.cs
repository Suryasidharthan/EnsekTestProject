using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Constraints;
using RestSharp;
using TechTalk.SpecFlow;
using System.Text.RegularExpressions;
using EnsekTestProject.EndPoints;
using EnsekTestProject.Models;

namespace EnsekTestProject.StepDefinitions
{
    [Binding]
    public class EnsekBuyFuelSteps
    {

        private RestClient client;
        private RestRequest request;
        private RestResponse response;
        private string accessToken;
        private List<string> fuelTypes = new List<string> { "3" };
        private EnsekBuyFuel buyFuel;
        private EnsekOrders viewOrders;
        private string baseUrl = "https://qacandidatetest.ensek.io/";

       public EnsekBuyFuelSteps() { viewOrders = new EnsekOrders(baseUrl); buyFuel = new EnsekBuyFuel(baseUrl); }
       

        [Given(@"I have a valid access token")]
        public void GivenIHaveAValidAccessToken()
        {
            accessToken = "eyJ0eXAiOiJ";
           
        }

        [When(@"I reset the test data")]
        public void WhenIResetTheTestData()
        {
            client = new RestClient(baseUrl);
            request = new RestRequest("/ENSEK/reset", Method.Post);
            request.AddHeader("Authorization", "Bearer " + accessToken);

            response = client.Execute(request);
        }

        [When(@"I buy a (.*) of fuelType having energy_id as (.*)")]
        public void WhenIBuyAOfFuelTypeHavingEnergy_IdAs(int quantity, int fuelId)
        {
            string actualOrderId;
            
             actualOrderId = buyFuel.BuyFuel(fuelId, quantity, accessToken);
            ScenarioContext.Current.Set(actualOrderId, "orderIdKey");
        }

        [Then(@"I verify that the above order is returned in the orders list with the expected (.*) and (.*) and Datetime details")]
        public void ThenIVerifyThatTheAboveOrderIsReturnedInTheOrdersListWithTheExpectedAndElectricDetails(int quantity, string fuelType)
        {
            {
                List<Order> listOfOrders = viewOrders.GetListOfOrders(accessToken);

                string actualOrderId = ScenarioContext.Current.Get<string>("orderIdKey");       //Get the orderId for the order  placed in the above step.
                foreach (var orderId in listOfOrders)
                {
                    Order order = listOfOrders.FirstOrDefault(o => o.id.Equals(actualOrderId)); //From the list of orders search for the order that was placed in the above step.
                    order.Should().NotBeNull($"Order with orderId {actualOrderId} is missing");
                    string actualFuel = order.fuel;
                    actualFuel.Should().Be(fuelType, $"The fuelType in the order {actualOrderId} is incorrect");
                    int actualQuantity = order.quantity;
                    actualQuantity.Should().Be(quantity, $"The quantity in the order {actualOrderId} is incorrect");

                    DateTime currentTime = DateTime.Now;

                    DateTime actualOrderTime = DateTime.Parse(order.time);

                    TimeSpan timeDifference = currentTime - actualOrderTime;

                    bool isTimeCorrect = timeDifference.TotalSeconds <= 30;

                    isTimeCorrect.Should().BeTrue($"Time on the order {actualOrderId} is incorrect");
                }

            }

        }


    }
}