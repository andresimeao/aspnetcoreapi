using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data
{
    public class ApiContext : DbContext
    { 
        public ApiContext(DbContextOptions<ApiContext> options): base(options) { } 
        public DbSet<User> Users{ get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Integrated Security=SSPI;Persist Security Info=False;User ID=andre;Initial Catalog=ecommerce;Data Source=DESKTOP-U1QEHIE\\SQLEXPRESS");
        }

        //serve para modelar as colunar do entidade.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("product");
        } 
    }
}