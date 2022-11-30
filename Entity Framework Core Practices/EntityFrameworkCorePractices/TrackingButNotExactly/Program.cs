using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

ETicaretContext context = new();

//ChangeTracker'ın davranışlarını yönetmemizi sağlayan metotlardır.

#region AsNoTracking Metodu
/*
Context üzerinden gelen tüm datalar ChangeTracker üzerinden takip edilmektedir.
ChangeTracker, takip ettiği nesnelerin sayısıyla doğru orantılı olacak şekilde bir maliyete sahiptir.
O yüzden üzerinde işlem yapılmayacak verilerin takip edilmesi bizlere lüzumsuz yere bir maliyet çıkaracaktır.

AsNoTracking metodu, context üzerinden sorgu neticesinde gelecek olan verilerin ChangeTracker tarafından takip 
edilmesini engeller.

AsNoTracking metodu ile ChangeTracker'ın ihtiyaç olmayan verilerdeki maliyetini törpülemiş oluruz.

AsNoTracking fonksiyonu ile yapılan sorgulamalarda, verileri elde edebililir, bu verileri istenilen noktalarda
kullanabilir lakin veriler üzerinde herhangi bir değişiklik/update işlemi yapamayız. Çünkü SaveChanges çağırıldığında
bu değişiklikleri yakalamış olan bir ChangeTracker mekanizmamız yok.
*/

//var kullanicilar = await context.Kullanicilar.ToListAsync(); maliyetli ve düşük performanslı
/*
var kullanicilar = await context.Kullanicilar.AsNoTracking().ToListAsync(); //daha verimli
foreach (var kullanici in kullanicilar)
{
    Console.WriteLine(kullanici.Adi);
    kullanici.Adi = $"yeni-{kullanici.Adi}"; //güncelleme yapılmayacaktır. Veriler ilk hali gibi kalacaktır.

    //illa ki güncelleme yapmak istiyorsak update fonksiyonu üzerinden güncelleme yapabiliriz.
    context.Kullanicilar.Update(kullanici); //context üzerinden Kullanicilar'a gidip ardından kullanici nesnesini
    //Update fonksiyonuna verirsek manuel güncelleme yapabiliriz.
}
await context.SaveChangesAsync();
*/
#endregion

#region AsNoTrackingWithIdentityResolution
/*
ChangeTracker mekanizması sayesinde yinelenen datalar aynı instance'ları kullanırlar. Mesela iki kullanıcı'ya
admin rolü verilecekse tek admin nesnesi üzerinden veriliyor. AsNoTracking durumunda ise iki tane tekrar eden
admin nesnesi oluşturulup kullanıcılara admin rolü veriliyor. AsNoTracking ile yapılan sorgularda yinelenen
datalar farklı instance'larda karşılanırlar. Çünkü ChangeTracker ile bağımız koptu.

AsNoTrackingWithIdentityResolution: ChangeTracker mekanizması yinelenen veerileri tekil instance olarak getirir.
Burada ekstradan performans kazancı söz konusudur. 

Bizler yaptığımız sorgularda takip mekanizmasının AsNoTracking metodu ile maliyetini kırmak isterken bazen
maliyete sebebiyet verebiliriz. Özellikle ilişkisel tabloları sorgularken bu duruma dikkat etmemiz gerekiyor.

AsNoTracking ile elde edilen veriler takip edilmeyeceğinden dolayı yinelenen verilerin ayrı instancelarda
olmasına sebebiyet veriyoruz. Çünkü ChangeTracker mekanizması takip ettiği nesneden bellekte varsa eğer 
aynı nesneden bir daha oluşturma gereği duymaksızın o nesneye ayrı noktalardaki ihtiyacı gidermektedir.

Böyle bir durumda hem takip mekanizmasının maliyetini ortadan kaldırmak hem de yinelenen dataları tek bir 
instance üzerinde karşılamak için AsNoTrackingWithIdentityResolution fonksiyonunu kullanabiliriz.
*/
/*
//var kullanicilar = await context.Kullanicilar.Include(k => k.Roller).ToListAsync(); //8 sonuç
//var kullanicilar = await context.Kullanicilar.Include(k => k.Roller).AsNoTracking().ToListAsync(); //10 sonuç

var kullanicilar = await context.Kullanicilar.Include(k => k.Roller).AsNoTrackingWithIdentityResolution().ToListAsync(); //9 sonuç
//yine 8 sonuç. En optimum çalışma.

//AsNoTrackingWithIdentityResolution fonksiyonu AsNoTracking fonksiyonuna nazaran görece yavaştır/maliyetlidir
//likin ChangeTracker'a nazaran daha performanslı ve az maliyetlidir.
*/
#endregion

#region AsTracking

/*Context üzerinden gelen dataların ChangeTracker tarafından takip edilmesini iradeli bir şekilde ifade etmemizi
sağlayan fonksiyondur. 

Bir sonraki inceleyeceğimiz UseQueryTrackingBehavior metodunun davranışı gereği uygulama
seviyesinde ChangeTracker'ın default olarak devrede olup olmamasını ayarlıyor olacağız. Eğer ki default olarak 
pasif hale getilirse böyle durumlarda takip mekanizmasının ihtiyaç olduğu sorgularda AsTracking fonksiyonunu 
kullanabilir ve böylece takip mekanizmasını iradeli bir şekilde devreye sokmuş oluruz.
*/

//var kitaplar = await context.Kitaplar.AsTracking().ToListAsync();

#endregion

#region UseQueryTrackingBehavior
//EF Core/uygulama seviyesinde ilgili context'ten gelen verilerin üzerinde ChangeTracker mekanizmasının
//davranışı temel seviyede belirlememizi sağlayan fonksiyondur. Yani konfigürasyon fonksiyonudur.
//public class ETicaretContext : DbContext'deki OnConfiguring fonksiyonunda ilgili context'in davranış modellemesini
//yapıyorduk.

#endregion

public class ETicaretContext : DbContext
{
    public DbSet<Kullanici> Kullanicilar { get; set; }
    public DbSet<Rol> Roller { get; set; }
    public DbSet<Kitap> Kitaplar { get; set; }
    public DbSet<Yazar> Yazarlar { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=PC\SQLEXPRESS;Database=ETicaretDB3;User ID=sa;Password=1;TrustServerCertificate=True;Trusted_Connection=true");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); //takip etme
        //Context üzerinden gelen datalar artık default olarak takip edilmeyeceğinden dolayı takip edilmesini
        //istediğimiz noktalarda AsTracking fonksiyonunu kullananmamız gerekecektir.

        //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution); //takip etme ama yinelenen dataların ayrı instance'larda olmasını engelle
        //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll); //default halidir. Takip edilmesini söyler.


    }

}
public class Kullanici
{
    public Kullanici() => Console.WriteLine("Kullanici nesnesi oluşturuldu.");

    public int Id { get; set; }
    public string Adi { get; set; }
    public float Email { get; set; }
    public string Password { get; set; }
    public ICollection<Rol> Roller { get; set; }
}
public class Rol
{
    public Rol() => Console.WriteLine("Rol nesnesi oluşturuldu.");

    public int Id { get; set; }
    public string RolAdi { get; set; }
    public ICollection<Kullanici> Kullanicilar { get; set; }
}
public class Kitap
{
    public Kitap() => Console.WriteLine("Kitap nesnesi oluşturuldu.");
    public int Id { get; set; }
    public string KitapAdi { get; set; }
    public int SayfaSayisi { get; set; }

    public ICollection<Yazar> Yazarlar { get; set; }
}
public class Yazar
{
    public Yazar() => Console.WriteLine("Yazar nesnesi oluşturuldu.");
    public int Id { get; set; }
    public string YazarAdi { get; set; }
    public ICollection<Kitap> Kitaplar { get; set; }
}