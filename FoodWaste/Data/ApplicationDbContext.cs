using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using FoodWaste.Models;
using Microsoft.AspNetCore.Identity;

namespace FoodWaste.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductAllergen>()
                .HasKey(nameof(ProductAllergen.AllergenId), nameof(ProductAllergen.ProductId));
            modelBuilder.Entity<UserAllergen>()
                .HasKey(nameof(UserAllergen.AllergenId), nameof(UserAllergen.UserId));
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<Restaurant> Restaurant { get; set; }
        public DbSet<UserAllergen> UserAllergens { get; set; }
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<ProductAllergen> ProductAllergens { get; set; }
        public DbSet<UserCreatedAllergens> UserCreatedAllergens { get; set; }
        public DbSet<UserSelectedAllergens> UserSelectedAllergens { get; set; }
        public DbSet<UserTemporaryProduct> UserTemporaryProducts { get; set; }
    }
}
