using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FoodWaste.Models;
using Microsoft.AspNetCore.Identity;

namespace FoodWaste.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>,IdentityRole<int>, int>//pa=iuret dokumentacija del sql
    {//nhibernate dapper
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //public DbSet<FoodWaste.Models.Product> Product { get; set; }
        //public DbSet<FoodWaste.Models.Restaurant> Restaurant { get; set; }
    }
}
