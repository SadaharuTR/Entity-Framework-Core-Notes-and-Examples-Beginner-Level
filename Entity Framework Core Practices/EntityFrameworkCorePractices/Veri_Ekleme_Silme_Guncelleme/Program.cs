using Microsoft.EntityFrameworkCore;

#region Veri Nasıl Silinir?
/*
ETicaretContext context = new();
Urun urun = await context.Urunler.FirstOrDefaultAsync(u => u.Id == 4); 
context.Urunler.Remove(urun); //4 id'sine sahip olan veriyi silmiş olduk
await context.SaveChangesAsync();   
*/
#endregion

#region Silme İşleminde ChangeTracker'ın Rolü

//ChangeTracker, context üzerinden gelen verilerin takibinden sorumlu bir mekanizmadır.
//Bu takip mekanizması sayesinde context üzerinden gelen verilerle ilgili işlemler neticesinde update yahut delete sorgularının oluşturulacağı anlaşılır.

#endregion

#region Takip Edilmeyen Nesneler Nasıl Silinir?
/*
ETicaretContext context = new ETicaretContext();

Urun u = new Urun
{
    Id = 2 //unique,primary key olan değeri yazılır.
};

context.Urunler.Remove(u);
await context.SaveChangesAsync();
*/
#endregion

#region EntityState ile Silme İşlemi
/*
ETicaretContext context = new ETicaretContext();

Urun u = new() { Id = 1 };
context.Entry(u).State = EntityState.Deleted;
await context.SaveChangesAsync();
*/
#endregion

#region SaveChanges'ı Verimli Kullanalım

#endregion

#region RemoveRange
/*
ETicaretContext context = new();
List<Urun> urunler = await context.Urunler.Where(u => u.Id >= 7 && u.Id <= 9).ToListAsync(); //id'sine 7'den büyük eşit ve 9'dan küçük eşit verileri getir.
context.Urunler.RemoveRange(urunler); //id 7,8,9'a sahip ürünler silindi.
await context.SaveChangesAsync();
*/
#endregion
public class ETicaretContext : DbContext
{
    public DbSet<Urun> Urunler { get; set; }
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
}


#region Veri Nasıl Güncellenir?
/*
ETicaretContext context = new();

Urun urun = await context.Urunler.FirstOrDefaultAsync(u => u.Id == 3); //vermiş olduğumuz şarta uyan ilk veriyi getirir.
//önce bu şekilde veriyi context üzerinden bir sorgulama ile elde ettik.

//şimdi nesne üzerinden güncelleyebiliriz.

urun.UrunAdi = "H Ürünü";
urun.Fiyat = 999;

//bu değişikliği veritabanına da bildirmemiz gerek.
await context.SaveChangesAsync();
*/
#endregion

#region ChangeTracker Nedir? Kısaca.

//ChangeTracker, context üzerinden gelen verilerin takibinden sorumlu bir mekanizmadır.
//Bu takip mekanizması sayesinde context üzerinden gelen verilerle ilgili işlemler neticesinde update yahut delete sorgularının oluşturulacağı anlaşılır.

#endregion

#region Takip Edilmeyen Nesneler Nasıl Güncellenir?
//EF Core'da Entity'ler üzerinden aşağıdaki şekilde oluşmuş olduğumuz nesneler direkt veritabanından elde edilmediği için, veritabanındaki nesne ile eşleştirilmez.
//çünkü bu nesne context üzerinden bir sorgulama neticesinde gelmedi. Bu yüzden ChangeTracker devreye girmedi.
//Bu takip edilmeyen nesneyi EF Core üzerinden güncellemek istiyorsak Update fonksiyonunu kullanabiliriz.
/*
ETicaretContext context = new ETicaretContext();

Urun urun = new()
{
    Id = 3,
    UrunAdi = "Yeni Ürün",
    Fiyat = 123
};
*/
#region Update Fonksiyonu 
/*
//ChangeTracker mekanizması tarafından takip edilmeyen nesnelerin güncellenebilmesi için Update fonksiyonu kullanılır.
//Update fonksiyonunu kullanabilmek için kesinlikle ilgili nesnede Id değeri verilmelidir!
//Bu değer güncellenecek(update sorgusu oluşturulacak) verinin hangisi olduğunu ifade edecektir.

//takip olmadığından dolayı SaveChanges'ı çağıramayız.
//Update ile EF Core'u bildirip,
context.Urunler.Update(urun);
//öyle SaveChanges'u çağırıyoruz.

await context.SaveChangesAsync();
*/
#endregion
#endregion

#region Entity State Nedir?
/*
//Bir entity instance'nın durumunu ifade eden bir referanstır.
ETicaretContext context = new();
Urun u = new();
Console.WriteLine(context.Entry(u).State); //Detached yazacaktır.
*/
#endregion

#region EF Core Açısından Bir Verinin Güncellenmesi Gerektiği Nasıl Anlaşılıyor?
/*
ETicaretContext context = new ETicaretContext();
Urun urun = await context.Urunler.FirstOrDefaultAsync(u => u.Id == 3);

Console.WriteLine(context.Entry(urun).State); //Unchanged yazacaktır.

urun.UrunAdi = "Domestos";
Console.WriteLine(context.Entry(urun).State); //Modified yazacaktır.

await context.SaveChangesAsync();
Console.WriteLine(context.Entry(urun).State); //Tekrardan Unchanged olacktır.
*/
#endregion

#region Birden Fazla Veri Güncellenirken Nelere Dikkat Edilmelidir?
/*
ETicaretContext context = new ETicaretContext();

var urunler = await context.Urunler.ToListAsync(); //ilgili DbSet'e karşılık gelen tabloya bir select sorgusu atıp tüm elemanlarını getir.

foreach (var urun in urunler)
{
    urun.UrunAdi += "*"; //her bir ürünün adının yanına * ekleyelim.
    //await context.SaveChangesAsync(); her iterasyonda çağırır ve çok maliyetli olur. Her bir ürüne karşılık bir transaction oluşturur.
}
await context.SaveChangesAsync(); //bir tane transaction oluşturur ve oluşturulan transaction içerisinde olması gereken update sorguları tek seferde işlenmiş olur.
*/
#endregion

#region Veri Ekleme
/*
using Microsoft.EntityFrameworkCore;

ETicaretContext context = new ETicaretContext();
Urun urun = new Urun()
{
    UrunAdi = "O Ürünü",
    Fiyat = 2000
};

await context.AddAsync(urun);
await context.SaveChangesAsync();
Console.WriteLine(urun.Id);

public class ETicaretContext : DbContext //ETicaretContext diye veritabanına karşılık gelen bir sınıfım var.
{
    public DbSet<Urun> Urunler { get; set; } //Bu veritabanı içerisinde de Urun entity'si modelinde bir tablo var adı da Urunler.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {        
        optionsBuilder.UseSqlServer(@"Server=PC\SQLEXPRESS;Database=ETicaretDB;User ID=sa;Password=1;TrustServerCertificate=True;Trusted_Connection=true");
    }
}

public class Urun //Entity isimleri tekil.
{    
    public int Id { get; set; }
    public string UrunAdi { get; set; }
    public float Fiyat { get; set; }
}

else
*/
#endregion

#region Veri Ekleme Pratikleri
//using Microsoft.EntityFrameworkCore;

//ETicaretContext context = new ETicaretContext();
//Urun urun1 = new Urun()
//{
//    UrunAdi = "C Ürünü",
//    Fiyat = 2000
//};

//Urun urun2 = new Urun()
//{
//    UrunAdi = "D Ürünü",
//    Fiyat = 2000
//};

//Urun urun3 = new Urun()
//{
//    UrunAdi = "E Ürünü",
//    Fiyat = 2000
//};

//await context.Urunler.AddRangeAsync(urun1, urun2, urun3);
//await context.SaveChangesAsync();

//-----------------------------

//using Microsoft.EntityFrameworkCore;

//ETicaretContext context = new ETicaretContext();
//Urun urun1 = new Urun()
//{
//    UrunAdi = "C Ürünü",
//    Fiyat = 2000
//};

//Urun urun2 = new Urun()
//{
//    UrunAdi = "D Ürünü",
//    Fiyat = 2000
//};

//Urun urun3 = new Urun()
//{
//    UrunAdi = "E Ürünü",
//    Fiyat = 2000
//};

//await context.AddAsync(urun1);
//await context.AddAsync(urun2);
//await context.AddAsync(urun3);

//await context.SaveChangesAsync();

//-------------------------

//using Microsoft.EntityFrameworkCore;

//ETicaretContext context= new ETicaretContext();
//Urun urun = new Urun()
//{
//    UrunAdi = "B Ürünü",
//    Fiyat = 2000
//};

//Console.WriteLine(context.Entry(urun).State); //Detached yazacaktır.

//await context.AddAsync(urun);
//Console.WriteLine(context.Entry(urun).State); 

//await context.SaveChangesAsync();
//Console.WriteLine(context.Entry(urun).State);

#endregion