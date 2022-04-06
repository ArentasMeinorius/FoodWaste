using FoodWaste.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace FoodWaste.Controllers
{
    public static class DataBaseOperations//paduot json body
    {
        private static string ProductUri = "https://localhost:44368/api/DB/product";
        private static string RestaurantUri = "https://localhost:44368/api/DB/restaurant";

        private static Lazy<Task<List<Product>>> ProductList = new Lazy<Task<List<Product>>>(() => UpdateProducts());

        public static async Task<List<Product>> GetProduct()
        {
            ProductList = new Lazy<Task<List<Product>>>(() => UpdateProducts());
            return await ProductList.Value;
        }
        public static async Task<List<Product>> UpdateProducts()
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            products.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(
            delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (Product prod in products)
                    {
                        if (prod.ExpiryDate < DateTime.Today)
                        {
                            prod.State = ProductState.Expired;
                            PutProduct(prod);
                        }
                    }
                }
            }
            );
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls;
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    using (var response = await httpClient.GetAsync(ProductUri))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var productResponse = JsonConvert.DeserializeObject<ObservableCollection<Product>>(apiResponse);
                        foreach (var p in productResponse)
                        {
                            products.Add(p);
                        }
                    }
                }
            }
            if (products.Count == 0)
            {
                throw new Exception("List is empty");
            }
            return products.ToList();
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
                    using (var response = await httpClient.PostAsync(ProductUri, content))
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
                    using (var response = await httpClient.PutAsync(ProductUri, content))
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
                    using (var response = await httpClient.DeleteAsync(ProductUri + "/" + id))
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
                    using (var response = await httpClient.GetAsync(RestaurantUri))
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
                    using (var response = await httpClient.PostAsync(RestaurantUri, content))
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
                    using (var response = await httpClient.PutAsync(RestaurantUri, content))
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
                    using (var response = await httpClient.DeleteAsync(RestaurantUri + "/" + id))
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }
    }
}
