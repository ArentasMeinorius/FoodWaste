using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FoodWaste.Models;

namespace FoodWaste.Data
{
    public class ApplicationDbContext : IdentityDbContext//pa=iuret dokumentacija del sql
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FoodWaste.Models.Product> Product { get; set; }
        public DbSet<FoodWaste.Models.Restaurant> Restaurant { get; set; }
    }
}
