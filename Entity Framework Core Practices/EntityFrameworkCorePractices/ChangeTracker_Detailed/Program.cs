using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

ETicaretContext context = new();
Console.WriteLine("a");

#region Change Tracking Nedir?
//Context nesnesi üzerinden gelen tüm nesneler/veriler otomatik olarak bir takip mekanizması tarafından izlenirler.
//İşte bu takip mekanizmasına Change Tracker denir. Change Tracker ile nesneler üzerindeki değişiklikler/işlemler takip edilerek netice itibariyle bu işlemlerin
//fıtratına uygun SQL sorgucukları generate edilir. İşte bu işlemene Change Tracking denir.
#endregion

#region ChangeTracker Property'si
//Takip edilen nesnelere erişlebilmemizi sağlayan ve gerektiği taktirde işlemler gerçekleştirmemizi sağlayan bir property'dir.
//Context sınıfının base class'ı olan DbContext sınıfının bir member'ıdır.
/*
var urunler = await context.Urunler.ToListAsync();
var datas = context.ChangeTracker.Entries(); //datas içerisinde takip edilen bütün değerleri/nesneleri Unchanged olarak görebiliriz.
*/
/*
var urunler = await context.Urunler.ToListAsync();
urunler[6].Fiyat = 123; //update
context.Urunler.Remove(urunler[7]); //delete
urunler[8].UrunAdi = "Corap";
//index no 6 ile 8'ın State'leri Modified; 7.'nin Deleted olacaktır.

var datas = context.ChangeTracker.Entries(); //burada değişiklikler tutuluyor ki,

await context.SaveChangesAsync(); //çağırıldığı zaman ChangeTracker'da tutulan state'lere uygun sorgular generate edilebilsin ve veritabanına göderilerek execute edilebilsin.

*/
#endregion

#region DetectChanges Metodu
/*
   Yapılan operasyonlarda güncel tracking verilerinden emin olabilmek için bu fonksiyonu kullanabiliriz.
EF Core, context nesnesi tarafından izlenen tüm nesnelerdeki değişiklikleri Chan ge Tracker sayesinde takip edebilmekte ve nesnelerde var olan verisel değişiklikler
yakalanarak bunların anlık görüntüleri(snapshot)'ini oluşturabilir.
   Yapılan değişikliklerin veritabanına gönderilmeden önce algılandığından emin olmak gerekir. SaveChanges fonksiyonu çağrıldığı anda nesneler EF Core tarafından otomatik
kontrol edilirler. ANcak yapılan operasyonlarda güncel tracking verilerinden emin olabilmek için değişikliklerin algılanmasında opsiyonel olarak gerçekleştirmek 
isteyebiliriz. İşte bunun için DetectChanges fonksiyonu kullanılabilir ve her ne kadar EF Core değişiklikleri otomatik algılıyor olsa da biz yine de irademizle
kontrole zorlayabiliriz.
 */
/*
var urun = await context.Urunler.FirstOrDefaultAsync(u => u.Id == 3);
urun.Fiyat = 123;

context.ChangeTracker.DetectChanges();
await context.SaveChangesAsync();
*/
#endregion

#region AutoDetectChangesEnabled Property'si
/*
    İlgili metotlar(SaveChanges, Entries) tarafından DetectChanges metodu otomatik olarak tetiklenmesinin konfigürasyonunu yapmamızı sağlayan property'dir.
    SaveChanges fonksiyonu tetiklendiğinde DetectChanges metodunu içerisinde default olarak çağırmaktadır. Bu durumda DetectChanges fonksiyonunun kullanımını irademizle
    yönetmek ve maliyet/performans optimizasyonu yapmak istediğimiz durumlarda AutoDetectChangesEnabled özelliğini kapatabiliriz.
*/
#endregion

#region Entries Metodu
/*
Context'teki Entry metodunun koleksiyonel versiyonudur.
ChangeTracker mekanizması tarafından izlenen her entity nesnesinin bilgisini EntityEntry türünden elde etmemizi sağlar ve belirli işlemler yapabilmemize olanak tanır.

Entries metodu, DetectChanges metodunu tetikler. Bu durumda tıpka SaveChanges'da olduğu gibi bir maliyettir.
Buradaki maliyetten kaçınmak için AutoDetectChangesEnabled özelliğine false değeri verilebilir.
*/
/*
var urunler = await context.Urunler.ToListAsync();
urunler.FirstOrDefault(u => u.Id == 7).Fiyat = 123; //update
context.Urunler.Remove(urunler.FirstOrDefault(u=> u.Id == 8)); //delete
urunler.FirstOrDefault(u => u.Id == 9).UrunAdi = "Corrrrap"; //update

context.ChangeTracker.Entries().ToList().ForEach(e =>
{
    if (e.State == EntityState.Unchanged)
    {
        //State'i Unchanged ise buradaki işlemleri uygula.
    }
    else if (e.State == EntityState.Deleted)
    {
        //State'i Deleted ise buradaki işlemleri uygula.
    }
});
*/
#endregion

#region AcceptAllChanges Metodu
/*
SaveChanges() veya SaveChanges(true) tetiklendiğinde EF Core herşeyin yolunda olduğunu varsayarak track ettiği verilerin takibini keser ve yeni değişikliklerin takip edilmesini
bekler. Böyle bir durumda beklenmeyen bir durum/olası bir hata söz konusu olursa eğer EF Core takip ettiği nesneleri bırakacağı için bir düzeltme metodu mevzu bahis olmayacaktır.

Haliyle bu durumda devreye SaveChanges(false) ve AcceptAllChanges metotları girecektir.

SaveChanges(false) EF Core'a gerekli veritabanı komutlarını yürütmesini söyler ancak gerektiğinde yeniden oynatılabilmesi için değişiklikleri beklemeye/nesneleri takip etmeye
devam eder. Ta ki AcceptAllChanges metodunu irademizle çağırana kadar.

SaveChanges(false) ile işlemin başarılı olduğundan emin olursanız AcceptAllChanges metodu ile nesnelerden takibi kesebilirsiniz.
*/
/*
var urunler = await context.Urunler.ToListAsync();
urunler.FirstOrDefault(u => u.Id == 7).Fiyat = 123; //update
context.Urunler.Remove(urunler.FirstOrDefault(u => u.Id == 8)); //delete
urunler.FirstOrDefault(u => u.Id == 9).UrunAdi = "Corrrrap"; //update

//await context.SaveChangesAsync();
//await context.SaveChangesAsync(true);

await context.SaveChangesAsync(false);
context.ChangeTracker.AcceptAllChanges();
*/
#endregion

#region HasChanges Metodu
//Takip edilen nesneler arasından değişiklik yapılanların olup olmadığının bilgisini verir. Arka planda DetectChanges metodunu tetikler.

//var result = context.ChangeTracker.HasChanges(); //sonuç olarak bool döner.

#endregion

#region Entity States 
//Entity nesnelerinin durumlarını ifade eder.
#region Detached
/*
//nesnenin change tracker mekanizması tarafından takip edilmediğini ifade eder.
Urun urun = new();
Console.WriteLine(context.Entry(urun).State); //detached yazacaktır. context üzerinden gelmiyor. ChangeTracker ile takip edilmiyor.
urun.UrunAdi = "Cururud";
await context.SaveChangesAsync();
*/
#endregion

#region Added
//veritabanına eklenecek nesneyi ifade eder. Henüz veritabanına işlenmeyen veriyi ifade eder.
//SaveChanges fonksiyonu çağırıldığında insert sorgusu oluşturulacağı anlamına gelir.
/*
Urun urun = new() { Fiyat = 123, UrunAdi = "Ürün 1001" };
Console.WriteLine(context.Entry(urun).State); //detached
await context.Urunler.AddAsync(urun); //şu andan itibaren takip söz konusu.
Console.WriteLine(context.Entry(urun).State); //added
await context.SaveChangesAsync();
urun.Fiyat = 312;
Console.WriteLine(context.Entry(urun).State); //Modified
await context.SaveChangesAsync();
*/
#endregion

#region Unchanged
//Veritabanında sorgulandığında beri nesne üzerinde herhangi bir değişiklik yapılmadığını ifade eder. Sorgu neticesinde elde edilen tüm nesneler başlangıçta
//bu state değerindedir.

//var urunler = await context.Urunler.ToArrayAsync();
//var data = context.ChangeTracker.Entries();

#endregion

#region Modified
//Nesne üzerinde değişiklik değişiklik ya da güncelleme yapıldığını ifade eder. SaveChanges fonksiyonu çağırıldığında update sorgusu oluşturulacağı anlamına gelir.
/*
var urun = await context.Urunler.FirstOrDefaultAsync(u => u.Id == 3);
Console.WriteLine(context.Entry(urun).State); //unchanged
urun.UrunAdi = "adibas";
Console.WriteLine(context.Entry(urun).State); //modified
await context.SaveChangesAsync();
Console.WriteLine(context.Entry(urun).State); //unchanged
*/
/*
var urun = await context.Urunler.FirstOrDefaultAsync(u => u.Id == 3);
Console.WriteLine(context.Entry(urun).State); //unchanged
urun.UrunAdi = "adibas";
Console.WriteLine(context.Entry(urun).State); //modified
await context.SaveChangesAsync(false);
Console.WriteLine(context.Entry(urun).State); //modified çünkü false yazdığımızdan dolayı AcceptAllChanged fonksiyonu manuel çalışmayı beklediğinden dolayı takip edilen
//nesneleri bırakmamıştır. Daha hala modified olarak kalmaya devam ediyor.
*/
#endregion

#region Deleted
//nesnenin silindiğini ifade eder. SaveChanges fonksiyonu çağırıldığı anda delete sorgusu oluşturulacağı anlamına gelir.
/*
var urun = await context.Urunler.FirstOrDefaultAsync(u => u.Id == 5);
context.Urunler.Remove(urun);
Console.WriteLine(context.Entry(urun).State);
context.SaveChangesAsync();
*/
#endregion

#endregion

#region Context Nesnesi Üzerinden Change Tracker
/*
context.ChangeTracker. ile o anda takip edilen bütün nesnelere
context.Entry. ile o anki nesneye.
*/
/*
var urun = await context.Urunler.FirstOrDefaultAsync(u => u.Id == 6);
urun.Fiyat = 123;
urun.UrunAdi = "Silgi"; //Modified | Update
*/
#region Entry Metodu

#region OriginalValues Propery'si
/*
var fiyat = context.Entry(urun).OriginalValues.GetValue<float>(nameof(Urun.Fiyat));
var urunAdi = context.Entry(urun).OriginalValues.GetValue<string>(nameof(Urun.UrunAdi));
*/
#endregion

#region CurrentValues Property'si

//var urunAdi = context.Entry(urun).CurrentValues.GetValue<string>(nameof(Urun.UrunAdi)); //ilgili instance'ın veritabanındaki değil Heap'teki değerini getirir.

#endregion

#region GetDatabaseValues Metodu

//var _urun = await context.Entry(urun).GetDatabaseValuesAsync(); //vermiş olduğumuz nesneye karışık veritabanından en güncel halini yine entity olarak elde etmemizi sağlar.

#endregion

#endregion

#endregion

#region Change Tracker'ın Interceptor Olarak Kullanılması
//public class ETicaretContext : DbContext 'a gidip override SaveChanges -> Cancellation -> TAB TAB yazalım.

#endregion

public class ETicaretContext : DbContext
{
    public DbSet<Urun> Urunler { get; set; }
    public DbSet<Parca> Parcalar { get; set; }
    public DbSet<UrunParca> UrunParcalari { get; set; }
    public DbSet<UrunDetay> UrunDetaylari { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=PC\SQLEXPRESS;Database=ETicaretDB3;User ID=sa;Password=1;TrustServerCertificate=True;Trusted_Connection=true");
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //Biz ne kadar SaveChanges tetiklersek burası tetiklenecek. Burası tetiklenirler şunları yapabiliriz.
        //veritabanına gönderilmeden önce araya girip operasyon gerçekleştirmek istiyorsak bu şekilde SaveChanges'ı override edebiliriz.

        var entries = ChangeTracker.Entries(); //entry'leri elde edip,

        foreach (var entry in entries)//gerçek base class'taki SaveChanges tetiklenmeden önce foreach ile buradaki entry'lere girip
        {
            if (entry.State == EntityState.Added) //buradaki entry'lerin durumlarına göre belirli operasyonları gerçekleştirebiliriz.
            {
                //mesela ekleme işlemi yapılıyorsa
                //entry.Entity... Entity üzerinden entry'e eriş ve işlemlere tabi tut şeklinde operasyonlar yapılabilir.
            }
        }
        return base.SaveChangesAsync(cancellationToken);//base class'taki SaveChanges 
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
    public float ParcaId { get; set; }
    public Urun Urun { get; set; }
    public Parca Parca { get; set; }
}
public class UrunDetay
{
    public int Id { get; set; }
    public float Fiyat { get; set; }
}