using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

Console.WriteLine("aaa");

#region Default Convention
/*
İki entity arasındaki ilişkiyi Navigation property'ler üzerinden çoğul (ICollection ya da List vs.) olarak kurmalıyız.
Default Convention'da Cross Table'ı manuel oluşturmak zorunda değiliz. EF Core tasarıma uygun bir şekilde cross table'ı kendisi otomatik basacak ve generate edecektir.
Ve oluşturulan cross table'ın içerisinde composite primary key'i de otomatik oluşturmuş olacaktır.

class Kitap
{
    public int Id { get; set; }
    public string KitapAdi { get; set; }
    public ICollection<Yazar> Yazarlar { get; set; }
}

class Yazar
{
    public int Id { get; set; }
    public int YazarAdi { get; set; }
    public ICollection<Kitap> Kitaplar { get; set; }
}
*/
#endregion

#region Data Annotations
/*
//Cross table manuel olarak oluşturulmak zorundadır.
//Entity'leri oluşturduğumuz cross table entity'si ile bire çok bir ilişki kurulmalıdır.
//Cross table'da composite primary key'i data annotations(Attributes'lar) ile manuel kuramıyoruz. Bunun için de Fluent API'da çalışma yapmamız gerekiyor.
//Cross table'a karşılık bir entity modeli oluşturuyorsak eğer bunu Context sınıfı içerisinde DbSet property'si olarak bildirmek mecburiyetinde değiliz.

class Kitap
{
    public int Id { get; set; }
    public string KitapAdi { get; set; }
    public ICollection<KitapYazar> Yazarlar { get; set; }
}

class KitapYazar
{      
    --------------
    Composite primary key oluşturabilmek için iki tane [Key] yazmamıza Data Annotations'ları kullanırken izin yok. FLuentAPI'ya uğramak zorundayız.
    [Key]
    public int KitapId { get; set; }
    [Key]
    public int YazarId { get; set; }   
    --------------
    
    Eğer ki KId ve YId olarak isimlendirme kuralları dışında çalışsaydık, yine KId ve YId'yi primary key olarak OnModelCreating'de atardı. Fakat
    Foreign Key olarak atamaz, onların yerin oromatik olarak KitapId ve YazarId oluşturur onları foreign key atardı. Bunu düzeltmek için

    [ForeignKey(nameof(Kitap))] //şeklinde tasarlamamız gerekirdi.
    public int KId { get; set; }
    [ForeignKey(nameof(Yazar))]
    public int YId { get; set; }
    public Kitap Kitap { get; set; }
    public Yazar Yazar { get; set; }
    --------------
    
    public int KitapId { get; set; }
    public int YazarId { get; set; }
    public Kitap Kitap { get; set; }
    public Yazar Yazar { get; set; }   
}

class Yazar
{
    public int Id { get; set; }
    public int YazarAdi { get; set; }
    public ICollection<KitapYazar> Kitaplar { get; set; }
}
class EKitapDbContext : DbContext
{
    public DbSet<Kitap> Kitaplar { get; set; }
    public DbSet<Yazar> Yazarlar { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=PC\SQLEXPRESS;Database=EKitapDB;User ID=sa;Password=1;TrustServerCertificate=True;Trusted_Connection=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //hem KitapId hem YazarId'ye primary key verebilmek için new ile bir anonim tür oluşturduk. HasKey ile de manuel bir şekilde primary key olarak atadık.
        //Ayrıca temel isimlendirme kuralları gereği EF Core KitapId ve YazarId'yi foreign key olarak da atayacaktır.
        modelBuilder.Entity<KitapYazar>()
            .HasKey(ky => new { ky.KitapId, ky.YazarId });
    }
}
*/
#endregion

#region FluentAPI
//Cross table manuel oluşturulmalıdır.
//DbSet olarak eklenmesine lüzum yoktur.
//Composite Primary Key HasKey metodu ile kurulmalı.
class Kitap
{
    public int Id { get; set; }
    public string KitapAdi { get; set; }
    public ICollection<KitapYazar> Yazarlar { get; set; }
}

//Cross Table
class KitapYazar
{      
    public int KitapId { get; set; }
    public int YazarId { get; set; }
    public Kitap Kitap { get; set; }
    public Yazar Yazar { get; set; }
}

class Yazar
{
    public int Id { get; set; }
    public int YazarAdi { get; set; }
    public ICollection<KitapYazar> Kitaplar { get; set; }
}
class EKitapDbContext : DbContext
{
    public DbSet<Kitap> Kitaplar { get; set; }
    public DbSet<Yazar> Yazarlar { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=PC\SQLEXPRESS;Database=EKitapDB;User ID=sa;Password=1;TrustServerCertificate=True;Trusted_Connection=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KitapYazar>()
            .HasKey(ky => new { ky.KitapId, ky.YazarId });

        modelBuilder.Entity<KitapYazar>()
            .HasOne(ky => ky.Kitap)
            .WithMany(k => k.Yazarlar)
            .HasForeignKey(ky => ky.KitapId);

        modelBuilder.Entity<KitapYazar>()
            .HasOne(ky => ky.Yazar)
            .WithMany(k => k.Kitaplar)
            .HasForeignKey(ky => ky.YazarId);
    }
}
#endregion
