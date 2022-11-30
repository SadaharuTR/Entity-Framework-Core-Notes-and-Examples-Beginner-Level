using Microsoft.EntityFrameworkCore;

ApplicationDbContext context = new();

#region One to One İlişkisel Senaryolarda Veri Güncelleme
#region Saving
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
//await context.SaveChangesAsync();
#endregion

#region 1. Durum | Esas Tablodaki Veriye Bağımlı Veriyi Değiştirme
/*
//Öncelikle hedef Person'u ilgili Person'a karşılık gelen adres bilgisiyle birlikte elde etmem lazım.
//Person person = await context.Persons.FirstOrDefaultAsync(p => p.Id == 1); bu şekilde Id'si 1 olan Person'u getir dedik. Ama adres bilgisi de lazım. Join yapıları lazım.

//Hedef Person'u elde ettik ve yanında adres bilgisini de getirmiş olduk.
Person? person = await context.Persons //? koyarak buradaki gelecek olan verinin nullable olabileğinin farkındayım, bilgin olsun compiler bey, demiş oluyoruz.(uyarıyı gidermek için)
    .Include(p => p.Address) //Include vermiş olduğumuz Navigation Property'e karşılık gelen tabloyla, arkada oluşturacağı select sorgusunda bir join işlemi yapar.
    .FirstOrDefaultAsync(p => p.Id == 1);
//EF Core'da Include'u kullanıyorsak vermiş olduğumuz NP her neyse ilgili verilere karşılık o NP'de değerler olacaktır. person. dediğimiz ilgili değerler gelecektir.
//ilgili Person'un adresinin nesnesi bağlanmış olacaktır -> person.Address

context.Addresses.Remove(person.Address); //eski adresi sildik
person.Address = new() //yerine yeni adres nesnesi ile yeni adres bilgisini ekledik.
{
    PersonAddress = "Yeni adres bilgisi."
};

await context.SaveChangesAsync();
*/
#endregion

#region 2. Durum | Bağımlı Verinin İlişkisel Olduğu Ana Veriyi Güncelleme
//Address? address = await context.Addresses.FindAsync(1); //1 Id'sine sahip olan adresi elde ettik.
//address.Id = 2; //Adresin içindeki Id kolonu bir key'e karşılık geldiğinden dolayı silinemez, değiştirilemez.
//await context.SaveChangesAsync(); //hata.

//Öncelikle mevcut olan bağımlı-dependent veriyi sil. Sildikten sonra SaveChanges'ı çağır. 
//Address? address = await context.Addresses.FindAsync(1); //adresi aldık.
//context.Addresses.Remove(address); //1 Id'sine sahip adresi sildik.
//await context.SaveChangesAsync();

//Ondan sonra tekrardan ilgili veriyi oluşturduktan sonra Principal veriyle bunu ilişkilendir. (Mevcut Person ile ilişkilendir.)
//Person? person = await context.Persons.FindAsync(2); //adresin bilgilerini yeniden oluştur, (adres InMemory'de duruyor, sadece tablodan silindi.)
//address.Person = person; //ve bunu person ile ilişkilendir.

//Adresimize yeni bir Person ekleyerek de ekleme operasyonumuzu gerçekleştirebiliriz.
/*
Address? address = await context.Addresses.FindAsync(2);
context.Addresses.Remove(address);
await context.SaveChangesAsync();

address.Person = new()
{
    Name = "Faruk"
};

await context.Addresses.AddAsync(address);

await context.SaveChangesAsync();
*/
#endregion
#endregion

#region One to Many İlişkisel Senaryolarda Veri Güncelleme
#region Saving
/*
Blog blog = new()
{
    Name = "noktadotkom.com Blog",
    Posts = new List<Post>
    {
        new(){ Title = "1. Post" },
        new(){ Title = "2. Post" },
        new(){ Title = "3. Post" },
    }
};

await context.Blogs.AddAsync(blog);
await context.SaveChangesAsync();
*/
#endregion

#region 1. Durum | Esas Tablodaki Veriye Bağımlı Verileri Değiştirme
//Önce Blog'u sonra Blog'la ilişkisel durumda olan Post'ları elde edeceğiz. (join işlemi)
/*
Blog? blog = await context.Blogs
    .Include(b => b.Posts) //Post'ları da sorguya ekle.
    .FirstOrDefaultAsync(b => b.Id == 1); //1 Id'sine sahip Blog'u elde et.

Post? silinecekPost = blog.Posts.FirstOrDefault(p => p.Id == 2); //2 Id'sine sahip silmek istediğimiz Post'u elde et.
blog.Posts.Remove(silinecekPost); //ilgili Post'u sil.

blog.Posts.Add(new() { Title = "4. Post" }); //4 ve 5 Post Title'larına sahip olan yeni Postları Blog'la ilişkilendir.
blog.Posts.Add(new() { Title = "5. Post" });

await context.SaveChangesAsync();
*/
#endregion

#region 2. Durum | Bağımlı verilerin ilişkisel olduğu ana veriyi güncelleme
//4.Post'u BlogId'si 2 olan Blog ile ilişkilendirelim. Yanlışlıkla 1'e gitmiş varsayıp 2'ye çevirelim.
/*
Post? post = await context.Posts.FindAsync(4); //hangi Post'un foreign key'ini güncelleyeceksek ona geliyoruz. 4 Id'sine sahip Post'u elde ettik.

post.Blog = new() //Blog 2 olmadığından dolayı yenisini oluşturalım.
{
    Name = "2. Blog"
};
//salt bir güncelleme olacağından dolayı direkt olarak SaveChanges'i çağırabiliriz. 2. Blog'u oluşturup ardından hangi Post'taysak onu da 2 ile güncelle.
await context.SaveChangesAsync();
*/

//Peki 5.Post'u da 2'ye vermek istersek? Yukarıdaki kodu çalıştırdığımız için artık 2. Blog mevcut.
/*
Post? post = await context.Posts.FindAsync(5); //5.Post'u elde et.
Blog? blog = await context.Blogs.FindAsync(2); //2.Blog'u elde et.
post.Blog = blog; //2.Blog'u 5.Post'a ver.
await context.SaveChangesAsync();
*/
#endregion
#endregion

#region Many to Many İlişkisel Senaryolarda Veri Güncelleme
#region Saving
/*
Book book1 = new() { BookName = "1. Kitap" };
Book book2 = new() { BookName = "2. Kitap" };
Book book3 = new() { BookName = "3. Kitap" };

Author author1 = new() { AuthorName = "1. Yazar" };
Author author2 = new() { AuthorName = "2. Yazar" };
Author author3 = new() { AuthorName = "3. Yazar" };

book1.Authors.Add(author1); //1.Kitapla 1 ve 2. Yazarları
book1.Authors.Add(author2);

book2.Authors.Add(author1); //2. Kitapla 1-2-3. Yazarları
book2.Authors.Add(author2);
book2.Authors.Add(author3);

book3.Authors.Add(author3); //3. Kitapla 3. Yazarı ilişkilendirip,

await context.AddAsync(book1); //Eklemeleri yaptık.
await context.AddAsync(book2);
await context.AddAsync(book3);
await context.SaveChangesAsync();
*/
#endregion

#region 1. Örnek: 1. Kitaba 3. Yazarı da eklemek-ilişkilendirmek istersek;
/*
Book? book = await context.Books.FindAsync(1); //1. kitabı elde et.
Author? author = await context.Authors.FindAsync(3); //3. yazarı elde et.
book.Authors.Add(author); //1. book'un Authors'una 3. Author'u ekle.

await context.SaveChangesAsync();
*/
#endregion
#region 2. Örnek :3 Id'sine sahip yazarın sadece 1 Id'sine sahip kitapla ilişkisi olsun. Diğer ilişkilerini koparalım.
/*
Author? author = await context.Authors
    .Include(a => a.Books) //Yazarın ilişkisi olduğu kitapları elde et.
    .FirstOrDefaultAsync(a => a.Id == 3); //3 Id'sine sahip olan yazarı elde et.

foreach (var book in author.Books) //foreach ile yazarın sahip olduğu bütün Books'lara girelim. Book'ları elde edip silelim. (tek tek manuel de yapılabilir bu işlem)
{
    if (book.Id != 1) //Id'si 1 değilse book'un
        author.Books.Remove(book); //Book'ları sil.
}

await context.SaveChangesAsync();
*/
#endregion

#region 3. Örnek: 2 Id'sine sahip kitabın, 1 Id'sine sahip yazarla ilişkisini keselim 3 Id'sine sahip yazarla ilişkisini ekleyelim. Ekstradan 4. Yazarı da ekleyelim.

Book? book = await context.Books //kitaptan yola çıktığımız için Book ile başlayalım.
    .Include(b => b.Authors) //İlgili book'a karşılık gelen tüm yazarları elde et.
    .FirstOrDefaultAsync(b => b.Id == 2); //2 Id'sine sahip Book'u elde et.

Author silinecekYazar = book.Authors.FirstOrDefault(a => a.Id == 1); //book. üzerinden 1 Id'sine sahip olan Author'u elde et.
book.Authors.Remove(silinecekYazar); //ve cross table'dan kaydını-ilişkisini sil-kopar.

Author eklenecekYazar = await context.Authors.FindAsync(3); //3 Id'sine sahip Author'u elde et. 
book.Authors.Add(eklenecekYazar); //book. üzerinden yeni ilişkiyi ekle.

//yeni 4.yazarı ekle ve 2 Id'sine sahhip kitapla ilişkilendir.
book.Authors.Add(new() { AuthorName = "4. Yazar" });

await context.SaveChangesAsync();

#endregion
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
    public int BlogId { get; set; }
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
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithOne(p => p.Address)
            .HasForeignKey<Address>(a => a.Id);
    }
}