using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Onsale.Common.Entities;
using OnSale.Web.Data.Entities;

namespace OnSale.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrdenDetail> OrdenDetails { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();

            modelBuilder.Entity<Country>(con => 
            {
                con.HasIndex("Name").IsUnique();
                con.HasMany(c => c.Departments).WithOne(d => d.Country).OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Department>(dep =>
            {
                dep.HasIndex("Name", "CountryId").IsUnique();
                dep.HasOne(d => d.Country).WithMany(c => c.Departments).OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<City>(cit =>
            {
                cit.HasIndex("Name", "DepartmentId").IsUnique();
                cit.HasOne(c => c.Department).WithMany(d => d.Cities).OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Product>().HasIndex(p => p.Name).IsUnique();
          
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2");


            base.OnModelCreating(modelBuilder);
        }




    }
}
