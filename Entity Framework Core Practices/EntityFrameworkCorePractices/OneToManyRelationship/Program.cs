using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("aaa");

#region Default Convention
/*
Default Convention yönetminde bire çok ilişkiyi kurarken foreign key kolonuna karşılık gelen bir property tanımlamak mecburiyetinde değiliz. Eğer tanımlamazsak EF Core bunu
kendisi tamamlayacak yok eğer tanımlarsak, tanımladığımızı baz alacaktır.
*/
/*
class Calisan //Dependent Entity
{
    public int Id { get; set; }
    public string Adi { get; set; }
    //public int DepartmanId { get; set; } yazılmış gibi SQL tarafında kendisi bir DepartmanId kolonu oluşturulacaktır.
    public Departman Departman { get; set; }    
}

class Departman
{
    public int Id { get; set; }
    public int DepartmanAdi { get; set;}

    public ICollection<Calisan> Calisanlar { get; set; }
}
*/
#endregion

#region Data Annonations
/*
Default Convention yönteminde foreign key kolonuna karşılık gelen property'i tanımladığmızda bu property ismi temel geleneksel entity tanımlama kurallarına uymuyorsa eğer
Data Annotations'lar ile müdahelede bulunabiliriz.
*/
/*
class Calisan //Dependent Entity
{
    public int Id { get; set; }

    [ForeignKey(nameof(Departman))]
    public int DId { get; set; }
    public string Adi { get; set; }
    public Departman Departman { get; set; }
}

class Departman
{
    public int Id { get; set; }
    public int DepartmanAdi { get; set; }

    public ICollection<Calisan> Calisanlar { get; set; }
}
*/
#endregion

#region FluentAPI
class Calisan //Dependent Entity
{
    public int Id { get; set; }
    public int DId { get; set; }
    public string Adi { get; set; }
    public Departman Departman { get; set; }
}

class Departman
{
    public int Id { get; set; }
    public int DepartmanAdi { get; set; }
    public ICollection<Calisan> Calisanlar { get; set; }
}
#endregion
class ESirketDbContext : DbContext
{
    public DbSet<Calisan> Calisanlar { get; set; }
    public DbSet<Departman> Departmanlar { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=PC\SQLEXPRESS;Database=ESirketDB;User ID=sa;Password=1;TrustServerCertificate=True;Trusted_Connection=true");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /* Bu şekilde kendisi otomatik bir DepartmanId foreign key kolonu oluşturacaktır.
        modelBuilder.Entity<Calisan>()
            .HasOne(c => c.Departman)
            .WithMany(d => d.Calisanlar);
        */
        //farklı isimde bir kolon oluşturmak istersek
        modelBuilder.Entity<Calisan>()
            .HasOne(c => c.Departman)
            .WithMany(d => d.Calisanlar)
            .HasForeignKey(e => e.DId);
    }
}