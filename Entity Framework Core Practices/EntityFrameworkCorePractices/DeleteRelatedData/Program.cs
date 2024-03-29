﻿using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

ApplicationDbContext context = new();

#region One to One İlişkisel Senaryolarda Veri Silme
/*
Person? person = await context.Persons //Persons'a git.
    .Include(p => p.Address) //Address ile join'leyip çek.
    .FirstOrDefaultAsync(p => p.Id == 1); //oradan da 1 Id'sine sahip Person hangisiyse elde et.

if (person != null) //person null değilse aşağıdaki operasyonu gerçekleştir. Olmayan bir şeyi silemeyiz sonuçta.
    context.Addresses.Remove(person.Address); //İlgili adresi sil.
//1 Id'sine sahip Person'un ilişkili adresini sildik.
await context.SaveChangesAsync();
*/
#endregion

#region One to Many İlişkisel Senaryolarda Veri Silme

//2 Id'sine sahip Post silinsin. 1 ve 3 kalsın.
/*
Blog? blog = await context.Blogs //hedef Blog hangisiyse onu elde etmemiz lazım.
    .Include(b => b.Posts) //İlgili Blog'a karşılık gelen bütün Post'ları sorguya ekle.
    .FirstOrDefaultAsync(b => b.Id == 1); //1 Id'sine sahip Blog.

Post? post = blog.Posts.FirstOrDefault(p => p.Id == 2); //2 Id'sine sahip Post'u elde et.

context.Posts.Remove(post); //Ve sil.
await context.SaveChangesAsync();
*/
#endregion

#region Many to Many İlişkisel Senaryolarda Veri Silme
//Buradaki işlemler cross table üzerinden yapılır. 
//1 kitabına karşılık gelen 2 Yazarını silmeye çalışalım. Burada Yazar komple silinmez. Cross Table'daki ilişkisi silinir.
/*
Book? book = await context.Books 
    .Include(b => b.Authors)
    .FirstOrDefaultAsync(b => b.Id == 1);

Author? author = book.Authors.FirstOrDefault(a => a.Id == 2);
//context.Authors.Remove(author); //Yazarı silmeye kalkar!
book.Authors.Remove(author); //bağı koparmış oluruz. 1 Id'sine sahip kitabın artık 2 Id'li yazarla bir ilişkisi yok.
await context.SaveChangesAsync();
*/
/*
Book? book = await context.Books
    .Include(b => b.Authors)
    .FirstOrDefaultAsync(b => b.Id == 2); //2 Id'sine sahip Book'u getir.

Author? author = book.Authors.FirstOrDefault(a => a.Id == 2); //Id'sine sahip olan Kitaptan 2 Id'sine sahip olan yazarı elde et.
context.Authors.Remove(author); //Yazarı silmeye kalkar! Hem ilişki hem de yazarı sildi. Veri kaybı söz konusu.
await context.SaveChangesAsync();
*/

#endregion

//Şu ana kadar Principle Table'daki herhangi bir verinin Dependent Table'daki ilişkisel verileri arasından birilerini silmeye çalışırken nasıl davranış sergileyeceğimizi gördük.
//Principle Table'daki bir veriyi silmeye çalışırsak;

#region Cascade Delete Yapılanması
//Aşağıdaki davranış modelleri Fluent API ile konfigüre edilebilmektedir.
#region Cascade
//Esas tablodan silinen veriyle karşı/bağımlı tabloda bulunan ilişkili verilerin silinmesini sağlar.
/*
Blog? blog = await context.Blogs.FindAsync(1);
context.Blogs.Remove(blog); //Hem Blog hem de ilişkili post'lar silindi.
await context.SaveChangesAsync();
*/
#endregion

#region SetNull
//Esas tablodan silinen veriyle karşı/bağımlı tabloda bulunan ilişkili verilere null değerin atanmasını sağlar.
/*
Blog? blog = await context.Blogs.FindAsync(1);
context.Blogs.Remove(blog); //Hem Blog hem de ilişkili post'lar silindi.
await context.SaveChangesAsync();
*/
//Aşağıda detaylı not:
//One to One senaryolarda eğer ki Foreign key ve Primary key kolonları aynı ise o zaman SetNull davranışını KULLANAMAYIZ!

#endregion

#region Restrict
//Esas tablodan herhangi bir veri silinmeye çalışıldığında o veriye karşılık dependent table'da ilişkisel veri/veriler varsa eğer bu sefer bu silme işlemini engellenmesini sağlar.
/*
Blog? blog = await context.Blogs.FindAsync(1);
context.Blogs.Remove(blog);
await context.SaveChangesAsync();
//hata alırız. çünkü ilişkisel verilerimiz var. Restrict koyduk. Silemeyiz.
*/
#endregion

#endregion

#region Saving Data
//Person person = new()
//{
//    Name = "Gençay",
//    Address = new()
//    {
//        PersonAddress = "Yenimahalle/ANKARA"
//    }
//};

//Person person2 = new()
//{
//    Name = "Hilmi"
//};

//await context.AddAsync(person);
//await context.AddAsync(person2);

//Blog blog = new()
//{
//    Name = "yildizkenter.com Blog",
//    Posts = new List<Post>
//    {
//        new(){ Title = "1. Post" },
//        new(){ Title = "2. Post" },
//        new(){ Title = "3. Post" },
//    }
//};

//await context.Blogs.AddAsync(blog);

//Book book1 = new() { BookName = "1. Kitap" };
//Book book2 = new() { BookName = "2. Kitap" };
//Book book3 = new() { BookName = "3. Kitap" };

//Author author1 = new() { AuthorName = "1. Yazar" };
//Author author2 = new() { AuthorName = "2. Yazar" };
//Author author3 = new() { AuthorName = "3. Yazar" };

//book1.Authors.Add(author1);
//book1.Authors.Add(author2);

//book2.Authors.Add(author1);
//book2.Authors.Add(author2);
//book2.Authors.Add(author3);

//book3.Authors.Add(author3);

//await context.AddAsync(book1);
//await context.AddAsync(book2);
//await context.AddAsync(book3);
//await context.SaveChangesAsync();
#endregion

class Person
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Address Address { get; set; }
}
class Address
{
    public int? Id { get; set; }
    public string PersonAddress { get; set; }

    public Person Person { get; set; }
}
class Blog
{
    public Blog()
    {
        Posts = new HashSet<Post>();
    }
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Post> Posts { get; set; }
}
class Post
{
    public int Id { get; set; }
    public int? BlogId { get; set; } //SetNull ile çalışabilmek için Nullable yapmamız lazım.
    public string Title { get; set; }

    public Blog Blog { get; set; }
}
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
    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=PC\SQLEXPRESS;Database=ApplicationDB;User ID=sa;Password=1;TrustServerCertificate=True;Trusted_Connection=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*Birebire ve bireçok durumlarında Cascade.
         
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithOne(p => p.Address)
            .HasForeignKey<Address>(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade); //Eğer ki Person'dan herhangi bir veri silinirse buna karşılık Address tablosunda ilişkisel data varsa onu da sil.

        //EF Core default olarak ilişkisel tablolar arasındaki silme davranışı Cascade olarak ayarlar.

        modelBuilder.Entity<Post>()
            .HasOne(p => p.Blog)
            .WithMany(b => b.Posts)
            .OnDelete(DeleteBehavior.Cascade); //Eğer ki Blog'dan herhangi bir veri silinirse buna karşılık Post tablosunda ilişkisel data varsa onu da sil.
        */

        /*
        //Birebir ve bireçok durumlarında SetNull

        //SetNull olarak değiştirdiğimizde yeni migration'u oluşturmayı unutma.
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithOne(p => p.Address)
            .HasForeignKey<Address>(a => a.Id);
        //.OnDelete(DeleteBehavior.SetNull); olmaz, hata verir. Birebir yapılarda SetNull vereceksek, Foreign Key kolonu ayrı bir property tarafından temsil edilmeli.

        //Cannot create the foreign key "FK_Addresses_Persons_Id" with the SET NULL referential action, because one or more referencing columns are not nullable.
        //Could not create constraint or index. See previous errors. Update-database dediğimizde bu hatayı alırız. Çünkü Id'yi hem Primary hem Foreign Key olarak kullandık.
        //İşte bu yüzden birebir durumlarında SetNull Kullanamayız.

        //bireçok durumlarında kullanılabilir.
        //Blog silindiği taktirde Post'un foreign key kolununa null basılması lazım bu durumda class Post'a gidip int? BlogId
        modelBuilder.Entity<Post>()
            .HasOne(p => p.Blog)
            .WithMany(b => b.Posts)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);//ilgili foreign key kolonu illa ki required olmak zorunda değil.
        */

        //Birebir ve bireçok'ta Restrict.
        /*
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithOne(p => p.Address)
            .HasForeignKey<Address>(a => a.Id);

        modelBuilder.Entity<Post>()
            .HasOne(p => p.Blog)
            .WithMany(b => b.Posts)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        */
        //Çokaçok ilişkide her daim silme davranışı cascade üzerinde kuruludur. Yukarıdaki davranuş modelleri EF Core'da çokaçok ilişkilerde izin verilen silme yöntemleri değildir.
    }
}