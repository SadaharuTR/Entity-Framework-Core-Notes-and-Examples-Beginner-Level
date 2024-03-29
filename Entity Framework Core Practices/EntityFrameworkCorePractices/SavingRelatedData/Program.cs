﻿using Microsoft.EntityFrameworkCore;
using System.Net;
using System;
using System.Net.Sockets;

Console.WriteLine("a");

ApplicationDbContext context = new();

#region One to One İlişkisel Senaryolarda Veri Ekleme
/*
#region 1. Yöntem : Principal Entity Üzerinden Dependet Entity Verisi Ekleme
//Eğer ki principal entity üzerinden ekleme gerçekleştiriliyorsa dependent entity nesnesi verilmek zorunda değildir!
//Fakat, dependent entity üzerinden ekleme işlemi gerçekleştiriliyorsa eğer burada principal entity'nin nesnesine ihtiyacımız zaruridir.
Person person = new();
person.Name = "Faruk";
person.Address = new() { PersonAddress = "Çinçin/ANKARA" };

await context.AddAsync(person);
await context.SaveChangesAsync();

#endregion

#region 2. Yöntem : Dependent Entity Üzerinden Principal Entity Verisi Ekleme

Address address = new()
{
    PersonAddress = "Yenişehir/Mersin",
    Person = new() { Name = "Hakan"}
};

await context.AddAsync(address);
await context.SaveChangesAsync();

#endregion

class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }  
}

class Address
{
    public int Id { get; set; }
    public string PersonAddress { get; set; }
    public Person Person { get; set; }  
}

class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=PC\SQLEXPRESS;Database=ApplicationDB;User ID=sa;Password=1;TrustServerCertificate=True;Trusted_Connection=true");
    }

    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithOne(p => p.Address)
            .HasForeignKey<Address>(a => a.Id);
    }
}
*/
#endregion

#region One to Many İlişkisel Senaryolarda Veri Ekleme

#region 1. Yöntem: Principal Entity Üzerinden Dependent Entity Verisi Ekleme
/*
//Nesne Referansı Üzerinden Ekleme

Blog blog = new() { Name = "yazilimciboy33.com Blog" }; //Nesne referansı üzerinden ilgili veri ile ilişkisel dependent entity'leri,
blog.Posts.Add(new() { Title = "Post 1" }); //bu şekilde ekleyebilmek istiyorsak eğer,
blog.Posts.Add(new() { Title = "Post 2" }); //Post'un null olmaması lazım. (hata verir)
blog.Posts.Add(new() { Title = "Post 3" }); //Bu Post'un Null olmaması içinde bir yerde nesnesinin üretilmiş olması lazım. (aşağıda constructor içerisinde)

await context.AddAsync(blog);
await context.SaveChangesAsync();

//Object Initializers Üzerinden Ekleme
Blog blog2 = new()
{
    Name = "A Blog",
    Posts = new HashSet<Post>()
    {
        new() { Title ="Post 4" },
        new() { Title ="Post 5" },
        new() { Title ="Post 6" }
    }
};
await context.AddAsync(blog2);
await context.SaveChangesAsync();
*/
#endregion

#region 2. Yöntem: Dependent Entity Üzerinden Principal Entity Verisi Ekleme
//Bire çok veri eklemede pek tercih edilen bir yöntem değildir.
/*
Post post = new()
{
    Title = "Post 42",
    Blog  = new() {  Name = "B Blog"}
};
await context.AddAsync(post);
await context.SaveChangesAsync();
*/
#endregion

#region 3. Yöntem: Foreign Key Kolonu Üzerinden Veri Ekleme
//1. ve 2. yöntemler hiç veri olmadığı durumlarda ilişkisel veri eklememizi sağlayan yöntemlerken,
//bu 3. yöntem ise önceden eklenmiş olan bir principal entity verisiyle yeni dependent entity'lerin ilişkisel olarak eşleştirilmesini sağlamaktadır.
/*
Post post = new Post()
{
    BlogId = 1,
    Title = "Post 44"
};
await context.AddAsync(post);
await context.SaveChangesAsync();
*/
#endregion
/*
class Blog
{
    public Blog()
    {
        Posts = new HashSet<Post>(); //Blog'un constructor'u içerisinde bu değeri, bir tane koleksiyonel değer olarak veriyoruz.

    //List yerine HashSet kullanılmasının sebebi daha performanslı olması.
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Post> Posts { get; set; } // Buradaki ICollection navigation property'sine bir değer vermemiz lazım.   

}

class Post
{
    public int Id { get; set; }
    public int BlogId { get; set; } 
    public string Title { get; set; }
    public Blog Blog { get; set; }
}

class ApplicationDbContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=PC\SQLEXPRESS;Database=ApplicationDB;User ID=sa;Password=1;TrustServerCertificate=True;Trusted_Connection=true");
    }

}
*/
#endregion

#region Many to MAny İlişkisel Senaryolarda Veri Ekleme

//1.Yöntem: n to n ilişkisi eğer ki default convention üzerinden tasarlanmışsa kullanılan bir yöntemdir.
/*
Book book = new()
{
    BookName = "A Kitabı",
    Authors = new HashSet<Author>()
    {
    new() { AuthorName = "Tolsoyewski" },
    new() { AuthorName = "Necip Fazıl Asabıyanık" },
    new() { AuthorName = "Halide Edip Şafak"}
    }
};

await context.AddAsync(book);
await context.SaveChangesAsync();

class Book
{
    public Book() 
    {
        Authors = new HashSet<Author>();
    }
    public int Id { get; set; }
    public string BookName { get; set; }
    public ICollection<Author> Authors { get; set; }
}
class Author
{
    public Author()
    {
       Books = new HashSet<Book>();
    }
    public int Id { get; set; }
    public string AuthorName { get; set; }
    public ICollection<Book> Books { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=PC\SQLEXPRESS;Database=KitapDB;User ID=sa;Password=1;TrustServerCertificate=True;Trusted_Connection=true");
    }
}
*/

//2.Yöntem: n t n ilişkisi eğer ki flunt api ile tasarlanmışsa kulllanılan bir yöntemdir.
/*
Author author = new()
{
    AuthorName = "Fatih Terimson",
    Books = new HashSet<AuthorBook>()
    {
        new() {BookId = 1}, //hem varolan verilere yeni verileri ilişkilendirmek, (cross table entity'si üzerinden de işlem yapılabilir)
        new() {Book = new() { BookName = "Parayla Şampuanlık" } } //hem de olmayan verileri ilişkisel bir şekilde eklemek istiyorsak.
    }
};

await context.AddAsync(author);
await context.SaveChangesAsync();
class Book
{
    public Book()
    {
        Authors = new HashSet<AuthorBook>();
    }
    public int Id { get; set; }
    public string BookName { get; set; }
    public ICollection<AuthorBook> Authors { get; set; }
}

class AuthorBook
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }
    public Book Book { get; set; }
    public Author Author { get; set; }
}
class Author
{
    public Author()
    {
        Books = new HashSet<AuthorBook>();
    }
    public int Id { get; set; }
    public string AuthorName { get; set; }
    public ICollection<AuthorBook> Books { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=PC\SQLEXPRESS;Database=KitapDB;User ID=sa;Password=1;TrustServerCertificate=True;Trusted_Connection=true");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorBook>()            
            .HasKey(ba => new { ba.AuthorId, ba.BookId });

        modelBuilder.Entity<AuthorBook>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.Authors)
            .HasForeignKey(ba => ba.BookId);

        modelBuilder.Entity<AuthorBook>()
            .HasOne(ba => ba.Author)
            .WithMany(b => b.Books)
            .HasForeignKey(ba =>ba.AuthorId);   
    }
}
*/
#endregion
