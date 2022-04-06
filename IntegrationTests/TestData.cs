using FoodWaste.Data;
using FoodWaste.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static FoodWaste.Data.ApplicationDbContext;

namespace IntegrationTests
{
    public class TestData
    {
        public static string Password = "Aaaa1;";

        public static IdentityUser<int> user = new() { Id = 9, UserName = "bbbb", Email = "ttt@ttt.ttt", PhoneNumber = "888", PasswordHash = "AQAAAAEAACcQAAAAECNxbMZhxGgw8unv5cJlZCrCMEmMOxjCwef7eHJQ8Y6VLcPTvDqfCqyQMbjqHz360A==", EmailConfirmed = true };

        public static Restaurant restaurant = new()
        {
            Id = 10,
            UserId = 9,
            Name = "tttt",
            Address = "tttttttt",
            PhoneNumber = "8888"
        };

        public static List<Product> products = new()
        {
            new Product
            {
                Id = 11,
                Name = "Test",
                ExpiryDate = new DateTime(),
                State = ProductState.Listed,
                RestaurantId = 10,
                UserId = null
            },
            new Product
            {
                Id = 12,
                Name = "TData",
                ExpiryDate = new DateTime(),
                State = ProductState.Listed,
                RestaurantId = 10,
                UserId = null
            }
        };
    }
}
