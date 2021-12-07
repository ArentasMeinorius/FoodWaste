using FoodWaste;
using FoodWaste.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class ProductControllerTests : IClassFixture<TestWebFactory<Program>>
    {

        //private HttpClient _client;

        private readonly TestWebFactory<Program> _factory;

        private readonly HttpClient _client;

        public ProductControllerTests(TestWebFactory<Program> factory)
        {
            _factory = factory;
            //var service = _factory.Services.GetService<>;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task ProductControllerIndex()
        {
            var response = await _client.GetAsync("/Products");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Find by name", responseString);
        }

        [Fact]
        public async Task ProductControllerDetails()
        {
            //var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Products/Details");

            //var values = new JObject();
            //values.Add("id", 1);
            //values.Add("name", "Test");
            //values.Add("expirydate", new DateTime());
            //values.Add("state", Product.ProductState.Listed.ToString());
            //values.Add("restaurantid", 1);
            //values.Add("userid", 1);
            //HttpContent content = new StringContent(values.ToString(), Encoding.UTF8, "application/json");

            //postRequest.Content = content;

            //var response = await _client.SendAsync(postRequest);

            var response = await _client.GetAsync("/Products/Details/10");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
        }

        [Fact]
        public async Task ProductControllerCreate()
        {
            var response = await _client.GetAsync("/Products/Create");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
        }

        [Fact]
        public async Task ProductControllerDelete()
        {
            var response = await _client.GetAsync("/Products/Delete");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}
