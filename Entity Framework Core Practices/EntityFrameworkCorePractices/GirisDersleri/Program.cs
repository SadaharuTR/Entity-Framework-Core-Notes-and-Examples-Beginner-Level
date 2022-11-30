using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello");
public class ETicaretContext : DbContext //ETicaretContext diye veritabanına karşılık gelen bir sınıfım var.
{
    public DbSet<Urun> Urunler { get; set; } //Bu veritabanı içerisinde de Urun entity'si modelinde bir tablo var adı da Urunler.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //Provider
        //ConnectionString
        //Lazy Loading vb.

        //context nesnesinin hangi veritabanına uygun bir biçimde bir çalışma sergileyeceği belli oldu.
        optionsBuilder.UseSqlServer("Server=PC\\SQLEXPRESS;Database=ETicaretDB;User ID=sa;Password=1"); 
    }
}

public class Urun //Entity isimleri tekil.
{
    //public int Id { get; set; }
    //public int ID { get; set; }
    //public int UrunId { get; set; }
    public int UrunID { get; set; }
    //Yukarıdakilerden birini tanımladığımız zaman EF Core tanıyıp primary key olarak belirleyecektir.
    //İleride bu şekilde tanımlanan property'ler dışındaki bir property'i de primary key atamayı göreceğiz.
}

