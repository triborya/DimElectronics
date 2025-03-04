using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin_de_Electronice
{
    public class ContextClass : DbContext
    {
        public ContextClass() : base("ContextConnection") { }

        public DbSet<User> User { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Subcategory> Subcategory { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Favorite> Favorite { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<Cart> Cart { get; set; }
    }
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public bool UserType { get; set; }

        List<Favorite> Favorite { get; set; }
        List<Sale> Sale { get; set; }
        List<Cart> Cart { get; set; }
    }

    public class Brand
    {
        public int BrandId { get; set; }
        public string Name { get; set; }

        List<Product> Product { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        List<Subcategory> Subcategory { get; set; }
    }

    public class Subcategory
    {
        public int SubcategoryId { get; set; }
        public string Name { get; set; }

        public int CategoryId { get; set; }
        Category Category { get; set; }
        List<Product> Product { get; set; }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int SubcategoryId { get; set; }
        Subcategory Subcategory { get; set; }

        public int BrandId { get; set; }
        Brand Brand { get; set; }

        List<Favorite> Favorite { get; set; }
        List<Sale> Sale { get; set; }
        List<Cart> Cart { get; set; }
    }
    
    public class Favorite
    {
        public int FavoriteId { get; set; }

        public int ProductId { get; set; }
        Product Product { get; set; }

        public int UserId { get; set; }
        User User { get; set; }
    }

    public class Sale
    {
        public int SaleId { get; set; }

        public int ProductId { get; set; }
        Product Product { get; set; }

        public int UserId { get; set; }
        User User { get; set; }
    }

    public class Cart
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }
        Product Product { get; set; }

        public int UserId { get; set; }
        User User { get; set; }
    }
    public class ProductView
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Subcategory { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string KindFavorite { get; set; }
        public string KindCart { get; set; }
    }
}
