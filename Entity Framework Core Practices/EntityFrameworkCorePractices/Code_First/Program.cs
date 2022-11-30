
using Microsoft.EntityFrameworkCore;
using System;

ECommerceDbContext context = new();
await context.Database.MigrateAsync();

public class ECommerceDbContext : DbContext //ECommerceDbContext diye bir veritabanı modellemiş olduk.
{
    public DbSet<Product> Products { get; set; } //ve bu veritabanı içerisinde Products isminde bir tablo olacağını,
    //bu tablonun modelinin de Product türünde olacağını belirttik.
    public DbSet<Customer> Customers { get; set; } //Artık EF Core açısından bir Entity modelidir.

    //bu veritabanı modellemesi hangi veritabanı sunucusuna uygun bir şekilde modellendi?

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //context nesnemizle ilgili temel ayarlamalarımızı yapmamızı sağlayan bir fonksiyondur.
    {
        //context'in hangi veritabanı sunucusuna migrate edileceği burada belirtilir.

        optionsBuilder.UseSqlServer("Server=PC\\SQLEXPRESS;Database=ECommerceDb;User Id=sa;Password=1;TrustServerCertificate=True"); 
        //NuGet üzerinden kütüphaneyi yüklemeyi unutmayalım.
    }
}

//Entity
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public float Price { get; set; }
}
//Entity
public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}