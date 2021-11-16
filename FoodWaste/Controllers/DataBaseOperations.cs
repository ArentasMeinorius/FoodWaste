﻿using FoodWaste.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace FoodWaste.Controllers
{
    public static class DataBaseOperations//paduot json body
    {
        public static async Task<List<Product>> GetProduct() 
        {
            List<Product> products = new List<Product>();
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls;
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44368/api/DB/product"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                    }
                }
            }
            return products;
        }
        public static async Task<string> PostProduct(Product product)
        {
            var values = new JObject();
            //values.Add("id", product.Id);
            values.Add("name", product.Name);
            values.Add("expirydate", product.ExpiryDate);
            values.Add("state", product.State.ToString());
            values.Add("restaurantid", product.RestaurantId);
            values.Add("userid", product.UserId);
            HttpContent content = new StringContent(values.ToString(), Encoding.UTF8, "application/json");
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls;

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    using (var response = await httpClient.PostAsync("https://localhost:44368/api/DB/product", content))
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }
        public static async Task<string> PutProduct(Product product)
        {
            var values = new JObject();
            values.Add("id", product.Id);
            values.Add("name", product.Name);
            values.Add("expirydate", product.ExpiryDate);
            values.Add("state", product.State.ToString());
            values.Add("restaurantid", product.RestaurantId);
            values.Add("userid", product.UserId);
            HttpContent content = new StringContent(values.ToString(), Encoding.UTF8, "application/json");
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls;

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    using (var response = await httpClient.PutAsync("https://localhost:44368/api/DB/product", content))
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }
        public static async Task<string> DeleteProduct(int id)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls;

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:44368/api/DB/product?id="+id))
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }
        public static async Task<List<Restaurant>> GetRestaurant()
        {
            List<Restaurant> restaurants = new List<Restaurant>();
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls;
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44368/api/DB/restaurant"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        restaurants = JsonConvert.DeserializeObject<List<Restaurant>>(apiResponse);
                    }
                }
            }
            return restaurants;
        }
        public static async Task<string> PostRestaurant(Restaurant restaurant)
        {
            var values = new JObject();
            //values.Add("id", restaurant.Id);
            values.Add("name", restaurant.Name);
            values.Add("phonenumber", restaurant.PhoneNumber);
            values.Add("userid", restaurant.UserId);
            values.Add("address", restaurant.Address);
            HttpContent content = new StringContent(values.ToString(), Encoding.UTF8, "application/json");
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls;

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    using (var response = await httpClient.PostAsync("https://localhost:44368/api/DB/restaurant", content))
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }
        public static async Task<string> PutRestaurant(Restaurant restaurant)
        {
            var values = new JObject();
            values.Add("id", restaurant.Id);
            values.Add("name", restaurant.Name);
            values.Add("phonenumber", restaurant.PhoneNumber);
            values.Add("userid", restaurant.UserId);
            values.Add("address", restaurant.Address);
            HttpContent content = new StringContent(values.ToString(), Encoding.UTF8, "application/json");
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls;

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    using (var response = await httpClient.PutAsync("https://localhost:44368/api/DB/restaurant", content))
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }
        public static async Task<string> DeleteRestaurant(int id)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls;

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:44368/api/DB/restaurant?id=" + id))
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }
    }
}
