﻿using FoodWaste;
using FoodWaste.Data;
using FoodWaste.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Build.BuildEngine;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace IntegrationTests
{
    public class TestWebFactory<TStartup> : WebApplicationFactory<Startup> //where TEntryPoint : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DefaultConnection");
                });
                services.AddSingleton<TestData>();
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                //using (var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    try
                    {
                        appContext.Database.EnsureCreated();

                        appContext.Product.AddRange(TestData.products); // Add 3 test players
                        appContext.SaveChanges();
                        appContext.Restaurant.Add(TestData.restaurant); // Add 3 test players
                        appContext.SaveChanges();
                        appContext.Users.Add(TestData.user); // Add 3 test players
                        appContext.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            });
        }
    }
}