using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;

ESirketDbContext context = new();

#region Default Convention
/*
Her iki entity'de Navigation Property ile birbirlerini tekil olarak referans ederek fiziksel bir ilişkinin olacağı ifade edilir. 

One to One ilişki türünde dependent entity'nin hangisi olduğunu default olarak belirleyebilmek pek kolay değildir. Bu durumda fiizksek olarak bir foreign key'e karşılık
property/kolon tanımlayarak çözüm getirebiliyoruz. Böylece foreign key' karşılık property tanımlayarak lüzumsuz bir kolon oluşturmuş oluyoruz.
*/
/*
class Calisan
{
    //Normal kolonlara karşılık gelen sıradan çinko karbon property'ler
    public int Id { get; set; }
    public string Adi { get; set; }

    //Eğer ilgili property'nin türü Entity'nin türünden ise Navigation property.
    public CalisanAdresi CalisanAdresi { get; set; }
}

class CalisanAdresi
{
    //CalisanAdresi'nin dependent olduğunu EF Core bildirmek istiyorsak.
    public int Id { get; set; } //bu bizim normal, primary key Id'miz.
    public int CalisanId { get; set; } //bu da bizim foreign key Id'miz olur. Artık CalisanAdresi dependent, Calisan ise Principal taraf olarak EF Core tarafından algılanacak.
    //bu default kurallar gereğidir.
    //CalisanId artık Calisanlar Tablosundaki Id ile ilişkilendirildi.
    public string Adres { get; set; }
    public Calisan Calisan { get; set; }
}
*/
#endregion

#region Data Annotations
/*
Navigation Property'ler tanımlanmalıdır.
Foreign Key kolonunun ismi default convention dışında bir kolon olacaksa eğer ForeignKey attribute'u ile bunu bildirebiliriz.
Foreign Key kolonu oluşturulmak zorunda değildir.
1'e 1 ilişkide ekstradan foreign key kolonuna ihtiyaç olmacağından olayı dependent entity'deki ıd kolonunu hem foreign hem de primary key olarak kullanmayı tercih ediyoruz.
*/
/*
class Calisan
{
    public int Id { get; set; }
    public string Adi { get; set; }

    public CalisanAdresi CalisanAdresi { get; set; }
}

class CalisanAdresi
{
    [Key, ForeignKey(nameof(Calisan))]
    public int Id { get; set; } //Id'yi hem primary key, hem de Foreign key atayarak tanımlayıp böylece index tanımlamadan unique olmuş oluyor ve 
    //ekstradan Foreign Key kolonu oluşturulmasına ihtiyaç kalmıyor. Maliyetten düşülüyor.
    //iki tablo arasındaki ilişkiyi birebir garantisine almış olduk. Calisan ve CalisanId kesinlikle aynı değere sahip olacak. Ve primary key olduğundan dolayı da unique olacaktır.

    
    //[ForeignKey(nameof(Calisan))]
    //public int Cevdet { get; set; } //CalisanAdresleri içerisindeki Cevdet, Calisanlar'daki Id ile ilişikili.
    //İsmini CalisanId'den değiştiğimiz an Data Annotations'a girmiş olduk. Fakat hala gereksiz bir kolon oluşturuluyor. Ve işe index'ler karışmış oluyor. Maliyetli!
    
    //public string Adres { get; set; }
    //public Calisan Calisan { get; set; }
}
*/
#endregion

/*
Navigation Property'ler tanımlanmalıdır.
FluentAPI yönteminde Entity'ler arasındaki ilişki  context sınıfı içerisinde OnModelCreating fonksiyonun override edilerek metotlar aracılığıyla tasarlanması gerekmektedir.
Tüm Sorumluluk bu fonksiyon içerisindeki çalışmalardadır.
*/

#region FluentAPI
class Calisan
{
    public int Id { get; set; }
    public string Adi { get; set; }

    public CalisanAdresi CalisanAdresi { get; set; }
}

class CalisanAdresi
{
    public int Id { get; set; } 
    public string Adres { get; set; }
    public Calisan Calisan { get; set; }
}
#endregion

class ESirketDbContext : DbContext
{
    public DbSet<Calisan> Calisanlar { get; set; }
    public DbSet<CalisanAdresi> CalisanAdresleri { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=PC\SQLEXPRESS;Database=ESirketDB;User ID=sa;Password=1;TrustServerCertificate=True;Trusted_Connection=true");
    }

    //Model'ların(entity) veritabanında generate edilecek yapıları bu fonksiyonda konfigüre edilir.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder parametresi üzerinden Calisan'a gidelim. Calisan'dan HasOne ile CalisanAdresi ile birebir ilişki kuralım.
        modelBuilder.Entity<Calisan>()
            .HasOne(c => c.CalisanAdresi) //buradaki c, Calisan'ı temsil ediyor. Calisan'ın içerisindeki CalisanAdresi property'si bizim navigation property'miz.HasOne diyerek bu navigation property'nin gösterdiği tabloya birebir ilişki kur demiş olduk.
            .WithOne(c => c.Calisan) //buradaki c, CalisanAdresi'nin referansı(temsili). Bu da CalisanAdresi içerisindeki Calisan adresi ile birebir ilişki kuran np'miz.
            .HasForeignKey<CalisanAdresi>(c => c.Id); //Foreign Key yapısını tanıtıp hangi Entity'nin dependent olduğunu belirtiyoruz. CalisanAdresi içerisindeki Id'nin foreign key
                                                      //olacağını bildiriyoruz. Fakat burada Primary Key özelliği ezildi. Aşağıda;

        modelBuilder.Entity<CalisanAdresi>() //CalisanAdresi'ndeki Id'nin primary key olduğunu bildiriyoruz. Artık hem primary key hem foreign key.
            .HasKey(c => c.Id);
    }
    //işte bu API metotları üzerinden olan çalışmaya FluentAPI deiyoruz.
}