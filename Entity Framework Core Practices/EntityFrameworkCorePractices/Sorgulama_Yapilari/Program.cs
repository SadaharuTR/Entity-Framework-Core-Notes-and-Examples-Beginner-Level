using Microsoft.EntityFrameworkCore;

ETicaretContext context = new();



#region Çoğul Veri Getiren Sorgulama Fonksiyonları

#region ToListAsync
//Üretilen sorguyu execute ettirmemizi sağlayan fonksiyondur.
/*
Method Syntax 
var urunler = context.Urunler.ToListAsync();
*/
//Query Syntax
//var urunler = (from urun in context.Urunler
//               select urun).ToListAsync();
//ya da
//var urunler = (from urun in context.Urunler
//               select urun);
//var datas = await urunler.ToListAsync();
#endregion

#region Where
/*
//Oluşturulan Sorguya Where şartı eklememizi sağlayan bir fonksiyondur.

//Method Syntax
var urunler = await context.Urunler.Where(u => u.Id > 500).ToListAsync();
var urunler2 = await context.Urunler.Where(u => u.UrunAdi.StartsWith("a")).ToListAsync();
var urunler3 = await context.Urunler.Where(u => u.UrunAdi.EndsWith("b")).ToListAsync();
var urunler4 = await context.Urunler.Where(u => u.UrunAdi.Contains("c")).ToListAsync();

//Query Syntax
var urunler5 = from urun in context.Urunler
               where urun.Id > 500 && urun.UrunAdi.EndsWith("7")
               select urun;

var data = await urunler5.ToListAsync();
*/
#endregion

#region OrderBy
//Sorgu üzerinde sıralama yapmamızı sağlayan bir fonksiyondur.
//Ascending olarak sıralamayı yapar.
/*
//Method Syntax
var urunler = context.Urunler.Where(u => u.Id > 500 || u.UrunAdi.StartsWith("2")).OrderBy(u => u.UrunAdi);

//Query Syntax
var urunler2 = from urun in context.Urunler
               where urun.Id > 500 || urun.UrunAdi.EndsWith("2")
               orderby urun.UrunAdi
               select urun;

await urunler.ToListAsync();
await urunler2.ToListAsync();
*/
#endregion

#region ThenBy
//OrderBy üzerinde yapılan sıralama işlemini farklı kolonlara da uygulamamızı sağlayan bir fonksiyondur. (Ascending)
/*
//Method Syntax
var urunler = context.Urunler.Where(u => u.Id > 500 || u.UrunAdi.StartsWith("2")).OrderBy(u => u.UrunAdi).ThenBy(u => u.Fiyat).ThenBy(u => u.Id);
await urunler.ToListAsync();
*/
#endregion

#region OrderByDescending
//Descending olarak sıralama yapmamızı sağlayan bir fonksiyondur.
/*
//Method Syntax
var urunler = await context.Urunler.OrderByDescending(u => u.Fiyat).ToListAsync();

//Query Syntax
var urunler2 = await (from urun in context.Urunler
                     orderby urun.UrunAdi descending
                     select urun).ToListAsync();
*/
#endregion

#region ThenByDescending
/*
//OrderByDescending üzerinde yapılan sıralama işlemini farklı kolonlara da uygulamamızı sağlayan bir fonksiyondur. (Ascending)
var urunler = await context.Urunler.OrderByDescending(u => u.Id).ThenByDescending(u => u.Fiyat).ThenBy(u => u.UrunAdi).ToListAsync();
*/
#endregion
#endregion

#region En Temel Basit Bir Sorgulama Nasıl yapılır?
/*
#region Method Syntax
//Sorgulama sürecinde metotları kullanırsak buna Method Syntax denir.
var urunler = await context.Urunler.ToListAsync();
#endregion

#region Query Syntax
//ürünler tablosunun içerisindeki her bir ürünü bana getir.
var urunler2 = await (from urun in context.Urunler //Urunler tablosunda yapılan bu sorgulamadaki her bir veriyi urun değişkeni ile (adı farklı olabilir) temsil edip select ile çek.
               select urun).ToListAsync();
#endregion
*/
#endregion

#region  Sorguyu Execute Etmek İçin Ne Yapmamız Gerekmektedir?
/*
//ToListAsync kullanabiliriz.
//ya da

int urunId = 5;
string urunAdi = "2";

var urunler = from urun in context.Urunler
              where urun.Id > urunId && urun.UrunAdi.Contains(urunAdi) //başka bir yerde urunAdi belirtmezsek, 2 barındıranları getirir.
              select urun;

urunId = 200; //ToListAsync'te de execute edilirken 200 değerine göre çalışmayı gerçekleştirecektir.
urunAdi = "4"; //artık 4 olanları getirecektir.

await urunler.ToListAsync();

/*foreach 
foreach (Urun urun in urunler)
{
    Console.WriteLine(urun.UrunAdi);
}
*/

#endregion

#region Deferred Execution (Ertelenmiş Çalışma)
//IQeryable çalışmalarında ilgili kod, yazıldığı anda tetiklenmez/çalıştırılmaz. Yani ilgili kod yazıldığı noktada sorguyu generate etmez! 
//Çalıştırıldığı/execute edildiği noktada tetiklenir. İşte buna ertelenmiş çalışma denir.
#endregion

#region IQueryable ve IEnumerable Nedir? Basit anlatım.
//IQueryable sorguya karşılık gelir. EF Core üzerinden yapılmış olan sorgunun execute edilmemiş halini ifade eder.
//IEnumerable sorgunun çalıştırılıp/execute edilip verilerin in memory'e yüklenmiş (artık veriler yazılımda entity instance'ları olarak tutuluyor) halini ifade eder.
/*
IQueryable
var urunler = from urun in context.Urunler
              select urun;
IEnumerable
var urunler = (from urun in context.Urunler
              select urun).ToListAsync();
*/
#endregion
public class ETicaretContext : DbContext
{
    public DbSet<Urun> Urunler { get; set; }
    public DbSet<Parca> Parcalar { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=PC\SQLEXPRESS;Database=ETicaretDB;User ID=sa;Password=1;TrustServerCertificate=True;Trusted_Connection=true");
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