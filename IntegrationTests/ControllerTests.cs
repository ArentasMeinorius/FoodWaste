using FoodWaste;
using FoodWaste.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class ControllerTests : IClassFixture<TestWebFactory<Startup>>
    {
        private readonly TestWebFactory<Startup> _factory;

        private readonly HttpClient _client;

        public ControllerTests(TestWebFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task ProductControllerIndex()
        {
            var response = await _client.GetAsync("/Products");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Test", responseString);
            Assert.Contains("TData", responseString);
        }

        [Fact]
        public async Task ProductControllerDetails()
        {
            var response = await _client.GetAsync($"/Products/Details/{TestData.products[0].Id}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Test", responseString);
            Assert.Contains("tttt", responseString);
            Assert.Contains("Restaurant Name", responseString);
            Assert.Contains("Product Name", responseString);
        }
        [Fact]
        public async Task ProductControllerDelete()
        {
            var response = await _client.GetAsync($"/Products/Delete/{TestData.products[1].Id}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Delete", responseString);
            Assert.Contains("TData", responseString);
        }
        [Fact]
        public async Task ProductControllerEdit()
        {
            var response = await _client.GetAsync($"/Products/Edit/{TestData.products[1].Id}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("State", responseString);
            Assert.Contains("TData", responseString);
        }

        internal async Task RestaurantControllerEdit()
        {
            var response = await _client.GetAsync("/Restaurants/Edit/10");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("tttt", responseString);
            Assert.Contains("tttttttt", responseString);
        }
        [Fact]
        public async Task RestaurantControllerDetails()
        {
            var response = await _client.GetAsync("/Restaurants/Details/9");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("tttt", responseString);
            Assert.Contains("tttttttt", responseString);
        }

        internal async Task RestaurantControllerIndex()
        {
            var response = await _client.GetAsync("/Restaurants");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("tttt", responseString);
            Assert.Contains("tttttttt", responseString);
            Assert.Contains("Restaurant Name", responseString);
            Assert.Contains("Address", responseString);
        }
        [Fact]
        public async Task RestaurantControllerCreate()
        {
            var response = await _client.GetAsync("/Restaurants/Create");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Name", responseString);
            Assert.Contains("Address", responseString);
        }
    }
}
