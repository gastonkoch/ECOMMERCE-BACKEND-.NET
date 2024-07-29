using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderNotification> OrderNotifications { get; set; }
        public DbSet<OrderProduct> OrdersProducts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder
            .Entity<User>()
            .Property(e => e.UserType)
            .HasConversion<int>();

            modelBuilder.Entity<User>().HasData(CreateCustomerSellerDataSeed());
            modelBuilder.Entity<Product>().HasData(CreateProductDataSeed());

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

            base.OnModelCreating(modelBuilder);
        }

        private User[] CreateCustomerSellerDataSeed()
        {
            User[] result = new User[]
            {
                    new User
                    {
                        Id = 1,
                        Name = "Gaston",
                        LastName = "Koch",
                        Password = "g",
                        Email = "gaston.koch@hotmail.com",
                        RegisterDate = DateOnly.FromDateTime(DateTime.ParseExact("06/06/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                        UserType = Domain.Enum.UserType.Seller
                    },
                    new User
                    {
                        Id = 2,
                        Name = "Alejandro",
                        LastName = "Di Stefano",
                        Password = "2",
                        Email = "distefanoalejandrom@gmail.com",
                        RegisterDate =  DateOnly.FromDateTime(DateTime.ParseExact("05/04/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                        UserType = Domain.Enum.UserType.Customer
                    },
                    new User
                    {
                        Id = 3,
                        Name = "Juan",
                        LastName = "Gomez",
                        Password = "3",
                        Email = "juan@gmail.com",
                        RegisterDate =  DateOnly.FromDateTime(DateTime.ParseExact("01/02/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                        UserType = Domain.Enum.UserType.Seller
                    },
                    new User
                    {
                        Id = 4,
                        Name = "Ana",
                        LastName = "Lopez",
                        Password = "4",
                        Email = "ana@gmail.com",
                        RegisterDate =  DateOnly.FromDateTime(DateTime.ParseExact("10/05/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                        UserType = Domain.Enum.UserType.Customer
                    },
                    new User
                    {
                        Id = 5,
                        Name = "Luis",
                        LastName = "Franco",
                        Password = "123",
                        Email = "luis@gmail.com",
                        RegisterDate =  DateOnly.FromDateTime(DateTime.ParseExact("15/03/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                        UserType = Domain.Enum.UserType.Seller
                    },
                    new User
                    {
                        Id = 6,
                        Name = "Pepe",
                        LastName = "Moscheti",
                        Password = "876",
                        Email = "pepe@gmail.com",
                        RegisterDate =  DateOnly.FromDateTime(DateTime.ParseExact("15/03/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                        UserType = Domain.Enum.UserType.Seller
                    },
                    new User
                    {
                        Id = 7,
                        Name = "admin",
                        LastName = string.Empty,
                        Password = "admin",
                        Email = "admin@ecommerce.com",
                        RegisterDate =  DateOnly.FromDateTime(DateTime.ParseExact("15/03/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                        UserType = Domain.Enum.UserType.Admin
                    },
                    new User
                    {
                        Id = 8,
                        Name = "Luis",
                        LastName = "Guevara",
                        Password = "433",
                        Email = "luisG@hotmail.com",
                        RegisterDate =  DateOnly.FromDateTime(DateTime.ParseExact("15/03/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                        UserType = Domain.Enum.UserType.Seller
                    }
            };
            return result;
        }

        private Product[] CreateProductDataSeed()
        {
            Product[] result;
            result = [
                new Product
                    {
                        Id = 1,
                        Name = "Remera",
                        Price = 100.00m,
                        Description = "Talle L color blanco",
                        Stock = 500,
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Pantalon",
                        Price = 200.00m,
                        Description = "Talle 44 jean",
                        Stock = 300,
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Zapatos",
                        Price = 300.00m,
                        Description = "Talle 40 de color negro",
                        Stock = 400,
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "Zapatos",
                        Price = 400.00m,
                        Description = "Talle 38 de color blanco",
                        Stock = 1500,
                    },
                    new Product
                    {
                        Id = 5,
                        Name = "Zapatos",
                        Price = 350.00m,
                        Description = "Talle 36 de color marron",
                        Stock = 1000,
                    },
                    new Product
                    {
                        Id = 6,
                        Name = "Buzo",
                        Price = 500.00m,
                        Description = "Talle XL de color azul",
                        Stock = 900,
                    },
                    new Product
                    {
                        Id = 7,
                        Name = "Buzo",
                        Price = 500.00m,
                        Description = "Talle L de color blanco",
                        Stock = 750,
                    },
                    new Product
                    {
                        Id = 8,
                        Name = "Zapatillas",
                        Price = 350.00m,
                        Description = "Talle 40 marca Nike",
                        Stock = 1200,
                    },];
            return result;
        }
    }
}
