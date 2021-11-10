using FoodWaste.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace FoodWaste.Controllers
{
    public static class DataBaseOperations
    {
        public static async Task<List<Product>> GetProducts() 
        {
            List<Product> products = new List<Product>();
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls;
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44368/api/ProductDB/get"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                    }
                }
            }
            return products;
        }
        public static async Task<List<User>> GetUsers()
        {
            List<User> users = new List<User>();
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls;
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44368/api/UserDB/get"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        users = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                    }
                }
            }
            return users;
        }
        public static async Task<List<Restaurant>> GetRestaurants()
        {
            List<Restaurant> restaurants = new List<Restaurant>();
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls;
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44368/api/RestaurantDB/get"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        restaurants = JsonConvert.DeserializeObject<List<Restaurant>>(apiResponse);
                    }
                }
            }
            return restaurants;
        }
    }
}
