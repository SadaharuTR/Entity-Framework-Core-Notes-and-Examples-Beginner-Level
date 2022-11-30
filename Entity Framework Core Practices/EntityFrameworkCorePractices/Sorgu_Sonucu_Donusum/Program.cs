
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

ETicaretContext context = new();
Console.WriteLine("a");

#region Sorgu Sonucu Dönüşüm Fonksiyonları
//Bu fonksiyonlar ile sorgu neticesinde elde edilen verileri isteğimiz doğrultusunda farklı türlerde projeksiyon edebiliyoruz.
#endregion

#region ToDictionaryAsync
//Sorgu neticesinde gelecek olan veriyi bir Dictionary olarak elde etmek/karşılamak istiyorsak eğer kullanabiliriz.
//Tolist ile aynı amaca hizmet etmektedir. Yani oluşturulan sorguyu execute edip neticesini alırlar. Fakat ToList gelen sorgu neticesini Entity türünde bir
//koleksiyona (List<TEntity>) dönüştürmekteyken, ToDictionary ise gelen sorgu neticesini Dictionary türünden bir koleksiyona dönüştürecektir.
//Dictionary; (key,value) formatında verileri tutmamızı sağlayan bir koleksiyon çeşidididir.
/*
var urunler = context.Urunler.ToDictionaryAsync(u => u.UrunAdi, u => u.Fiyat);
*/
#endregion

#region ToArrayAsync
//Oluşturulan sorguyu dizi olarak elde eder. ToList ile aynı amaca hizmet eder. Sorguyu execute eder lakin gelen sonucu entity dizisi olarak elde eder.
/*
var urunler = context.Urunler.ToArrayAsync(); //üzerine sorgu çekmiş olduğumuz DbSet Urunler property'sine onun generic türüne uygun bir dizide sonuç döndürecektir.
*/
//ToArray,ToList,ToDictionary'de de SQL kısmında aynı sorgu oluşturulur. 
#endregion

#region Select
//Select fonksiyonunun işlevsel olarak birden fazla davranışı söz konusudur.
/*
//1) Select fonksiyonu, generate edilecek sorgunun çekilecek kolonlarını ayarlamamızı sağlamaktadır.
//ToList,ToArray,ToDictionary'den önce (IQueryable iken) kullanmamız lazım.
var urunler = context.Urunler.Select(u => new Urun
{
    Id = u.Id,
    Fiyat = u.Fiyat,
//sadece Id ve Fiyat kolonlarını getirir. Diğer kolonlar null olarak kalacaktır.
}).ToListAsync();

//2) Select fonksiyonu, gelen verileri farklı türlerde karşılamamızı sağlar. T, anonim gibi.
var urunler2 = context.Urunler.Select(u => new
{
    Id = u.Id,
    Fiyat = u.Fiyat,
    //bu şekilde de elde edilen veriler Anonymous Type olarak elde edilecektir.
}).ToListAsync();

var urunler3 = context.Urunler.Select(u => new UrunDetay
{
    Id = u.Id,
    Fiyat = u.Fiyat,
//bu şekilde de elde edilen veriler UrunDetay türünde elde edilecektir.
}).ToListAsync();
*/
#endregion

#region SelectMany
//Select ile aynı amaca hizmet eder. Lakin, ilişkisel tablolar neticesinde gelen koleksiyonel verileri de tekilleştirip projeksiyon etmemizi sağlar.
//SQL'deki inner join yapılanmasını kullanır.
/*
SELECT [u].[Id], [u].[Fiyat], [p].[ParcaAdi]
FROM [Urunler] AS [u]
INNER JOIN [Parcalar] AS [p] ON [u].[Id] = [p].[UrunId]
*/
/*
var urunler = await context.Urunler.Include(u => u.Parcalar).SelectMany(u => u.Parcalar, (u,p) => new
{
    u.Id,
    u.Fiyat,
    p.ParcaAdi
}).ToListAsync();
Console.WriteLine("a");
*/
#endregion

#region GroupBy Fonksiyonu
//Gruplama yapmamızı sağlayan fonksiyondur.
/*
SELECT Fiyat, COUNT(*) From Urunler
GROUP By Fiyat
*/
#region Method Syntax
/*
var datas = await context.Urunler.GroupBy(u => u.Fiyat).Select(group => new
{
    Count = group.Count(),
    Fiyat = group.Key
}).ToListAsync();

// SQL Server Profiler'a bakarsak;
SELECT COUNT(*) AS [Count], [u].[Fiyat]
FROM [Urunler] AS [u]
GROUP BY [u].[Fiyat]
*/
#endregion

#region Query Syntax
/*
var datas = await (from urun in context.Urunler
           group urun by urun.Fiyat
           into @group //gruplama işlemi neticesinde elde edilen veriyi group ile temsil et. @ verbatim ile keyword olmaktan çıkart.
           select new
           {
               Fiyat = @group.Key,
               Count = @group.Count()
           }).ToListAsync();
*/
/*
SELECT [u].[Fiyat], COUNT(*) AS [Count]
FROM [Urunler] AS [u]
GROUP BY [u].[Fiyat]
*/
#endregion
#endregion

#region Foreach Fonksiyonu
//Bir sorgulama fonksiyonu değildir.
//Sorgulama neticesinde elde edilen koleksiyonel veriler üzerinde iterasyonel olarak dönmemizi ve teker teker verileri elde edip işlemler yapabilmemizi sağlayan bir fonksiyondur.
//Foreach döngüsünün metot halidir.
/*
var datas = await (from urun in context.Urunler
                   group urun by urun.Fiyat
           into @group //gruplama işlemi neticesinde elde edilen veriyi group ile temsil et. @ verbatim ile keyword olmaktan çıkart.
                   select new
                   {
                       Fiyat = @group.Key,
                       Count = @group.Count()
                   }).ToListAsync();

foreach (var item in datas)
{

}
//ya da
datas.ForEach(x =>
{

});
*/
#endregion
public class ETicaretContext : DbContext
{
    public DbSet<Urun> Urunler { get; set; }
    public DbSet<Parca> Parcalar { get; set; }
    public DbSet<UrunParca> UrunParcalari { get; set; }
    public DbSet<UrunDetay> UrunDetaylari { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=PC\SQLEXPRESS;Database=ETicaretDB2;User ID=sa;Password=1;TrustServerCertificate=True;Trusted_Connection=true");
    }
}
public class Urun
{
    public int Id { get; set; }
    public string UrunAdi { get; set; }
    public float Fiyat { get; set; }
    public ICollection<Parca> Parcalar { get; set; }
}
public class Parca
{
    public int Id { get; set; }
    public string ParcaAdi { get; set; }
}

public class UrunParca
{
    [Key]
    public int UrunId { get; set; } 
    public float ParcaId { get; set;}
    public Urun Urun { get; set; }
    public Parca Parca  { get; set; }
}

public class UrunDetay
{
    public int Id { get; set; }
    public float Fiyat { get; set; }
}