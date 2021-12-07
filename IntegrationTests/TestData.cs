using FoodWaste.Data;
using FoodWaste.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FoodWaste.Data.ApplicationDbContext;

namespace IntegrationTests
{
    public class TestData
    {
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly ApplicationDbContext _context;

        public static string Password = @"Aaaa1;";

        public IdentityUser<int> user = new() { Id = 9, UserName = "bbbb", Email = "ttt@ttt.ttt", PhoneNumber = "888"};
        public Restaurant restaurant = new()
        {
            Id = 10,
            UserId = 9,
            Name = "tttt",
            Address = "tttttttt",
            PhoneNumber = "8888"
        };
        public List<Product> products = new()
        {
            
                new Product
                        {
                            Id = 11,
                            Name = "Test",
                            ExpiryDate = new DateTime(),
                            State = Product.ProductState.Listed,
                            RestaurantId = 10,
                            UserId = null
                        },
                new Product
                        {
                            Id = 12,
                            Name = "Test",
                            ExpiryDate = new DateTime(),
                            State = Product.ProductState.Listed,
                            RestaurantId = 10,
                            UserId = null
                        }
            };

        public TestData(ApplicationDbContext context, UserManager<IdentityUser<int>> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task Fill() 
        {
            await _userManager.CreateAsync(user, Password);
            _context.Add(restaurant);
            await _context.SaveChangesAsync();
            _context.AddRange(products);
            await _context.SaveChangesAsync();
        }
    }
}
