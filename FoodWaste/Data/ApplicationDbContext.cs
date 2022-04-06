using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using FoodWaste.Models;
using Microsoft.AspNetCore.Identity;

namespace FoodWaste.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>,IdentityRole<int>, int>
    {//nhibernate dapper
        public class ApplicationUser : IdentityUser<int>
        {
            public virtual ICollection<Product> Products { get; set; }
            public virtual ICollection<Restaurant> Restaurants { get; set; }
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<Restaurant> Restaurant { get; set; }
    }
}
