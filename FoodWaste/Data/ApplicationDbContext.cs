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
            modelBuilder.Entity<UserCreatedAllergens>()
                .HasKey(nameof(UserCreatedAllergens.AllergenId), nameof(UserCreatedAllergens.UserId));
            modelBuilder.Entity<UserSelectedAllergens>()
                .HasKey(nameof(UserSelectedAllergens.AllergenId), nameof(UserSelectedAllergens.UserId));
            modelBuilder.Entity<UserCreatedAllergens>()
                .Property(b => b.UserId)
                .ValueGeneratedNever();
            modelBuilder.Entity<UserSelectedAllergens>()
                .Property(b => b.UserId)
                .ValueGeneratedNever();
            modelBuilder.Entity<UserTemporaryProduct>()
                .Property(b => b.UserId)
                .ValueGeneratedNever();
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<Restaurant> Restaurant { get; set; }
        public DbSet<UserAllergen> UserAllergens { get; set; }
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<ProductAllergen> ProductAllergens { get; set; }
        public DbSet<UserCreatedAllergens> CreatedAllergens { get; set; }
        public DbSet<UserSelectedAllergens> SelectedAllergens { get; set; }
        public DbSet<UserTemporaryProduct> TemporaryProducts { get; set; }
    }
}
