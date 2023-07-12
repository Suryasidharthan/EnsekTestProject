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
using Microsoft.Extensions.Configuration;
using EnsekTestProject.Helpers;


namespace EnsekTestProject.StepDefinitions
{
    [Binding]
    public class EnsekBuyFuelSteps
    {

        private RestClient _client;
        private RestRequest _request;
        private string _baseUrl;
        private string _accessToken;
        private BuyFuel _buyFuel;
        private Orders _viewOrders;       
        private Login _login;
        private Reset _reset;
        

       public EnsekBuyFuelSteps()
        {
            try
            {
                var config = ConfigHelper.GetConfiguration();
                _baseUrl = config["BaseUrl"];
                _login = new Login(_baseUrl, config["username"], config["password"]);               //Login with the username password and baseUrl mentioned in config file
            }

            catch (Exception ex)
            {
                                                    
                Console.WriteLine($"Exception occurred while retrieving configuration: {ex}");        // Log or handle the exception appropriately
            }

        }
       
        [Given(@"I have a valid login with access token")]
        public void GivenIHaveAValidLoginWithAccessToken()
        {
            _accessToken = _login.GetAccessToken();
        }


        [When(@"I reset the test data")]
        public void WhenIResetTheTestData()
        {
            _reset = new Reset(_baseUrl);
            bool isResetSuccessful = _reset.ResetTestData(_accessToken);
            isResetSuccessful.Should().BeTrue("Reset not successful");
        }

        [When(@"I buy some (.*) of fuelType having energy_id as (.*)")]
        public void WhenIBuySomeOfFuelTypeHavingEnergy_IdAs(int quantity, int fuelId)
        {
            _buyFuel = new BuyFuel(_baseUrl);
            string actualOrderId;
            actualOrderId = _buyFuel.BuyFuelWithFuelIdAndQuantity(fuelId, quantity, _accessToken); //Buy fuel for the given FuelType and as per the quantity specified.
            ScenarioContext.Current.Set(actualOrderId, "orderIdKey");           //Save the orderId of the above order in the ScenarioContext to be used in the Then statement below.
        }

        [Then(@"I verify that the above order is returned in the orders list with the expected (.*) and electric and current Datetime details")]
        public void ThenIVerifyThatTheAboveOrderIsReturnedInTheOrdersListWithTheExpectedAndElectricAndCurrentDatetimeDetails(int p0)
        {
            throw new PendingStepException();
        }

        [Then(@"I verify that the above order is returned in the orders list with the expected (.*) and (.*) and the current Datetime details")]
        public void ThenIVerifyThatTheAboveOrderIsReturnedInTheOrdersListWithTheExpectedFuelTypeAndQuantityAndCurrentDatetimeDetails(string fuelType, int quantity)
        {
            {
                _viewOrders = new Orders(_baseUrl);
                List<Order> listOfOrders = _viewOrders.GetListOfOrders(_accessToken);

                string actualOrderId = ScenarioContext.Current.Get<string>("orderIdKey");       //Get the orderId for the order  placed in the above step.
                foreach (var orderId in listOfOrders)
                {
                    Order order = listOfOrders.FirstOrDefault(o => o.id.Equals(actualOrderId)); //From the list of orders search for the order that was placed in the above step.
                    order.Should().NotBeNull($"Order with orderId {actualOrderId} is missing"); //Verify that order is present in the list.

                    //Verify that the order is placed for the correct Fuel type.
                    string actualFuel = order.fuel;
                    actualFuel.Should().Be(fuelType, $"The fuelType in the order {actualOrderId} is incorrect");

                    //Verify that the quantity of order placed is as mentioned in the request.
                    int actualQuantity = order.quantity;
                    actualQuantity.Should().Be(quantity, $"The quantity in the order {actualOrderId} is incorrect");

                    //Verify that the difference between the orderTime and currentTime is less than 30 seconds(this threshold can vary)
                    DateTime currentTime = DateTime.Now;
                    DateTime actualOrderTime = DateTime.Parse(order.time);
                    TimeSpan timeDifference = currentTime - actualOrderTime;
                    bool isTimeCorrect = timeDifference.TotalSeconds <= 30;
                    isTimeCorrect.Should().BeTrue($"Time on the order {actualOrderId} is incorrect");
                }

            }

        }

        [Then(@"I verify that the above order is not placed and it is not returned in the orders list\.")]
        public void ThenIVerifyThatTheAboveOrderIsNotPlacedAndItIsNotReturnedInTheOrdersList_()
        {
            throw new PendingStepException();
        }



    }
}