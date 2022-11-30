using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

Console.WriteLine("a");

#region Relationships(İlişkiler) Terimleri

#region Principal Entity(Asıl Entity)
//Kendi başına var olabilen tabloyu modelleyen entity'e denir. Departmanlar tablosunu modelleyen 'Departman' entity'sidir.
#endregion

#region Dependent Entity (Bağımlı Entity)
//Kendi başına var olamayan, bir başka tabloya ilişkisel olarak bağımlı olan tabloyu modelleyen entity'e denir.
//Çalışanlar tablosunu modelleyen 'Calisan' entity'sidir.
#endregion

#region Foreign Key
//Principle Entity ile Dependent Entity arasındaki ilişkiyi sağlayan key'dir.
//Dependent Entity'de tanımlanır.
//Principal Entity'deki Principal Key'i tutar.
//Calisanlar'daki DepartmanId, Departmanlar'daki Id'ye karşılık geliyor.
#endregion

#region Principal Key
//Principal Entity'deki id'nin kendisidir. Principal Entity'nin kimliği olan kolonu ifade eden property'dir.
#endregion

#region Navigation Property Nedir?
//İlişkisel tablolar arasındaki fiziksel erişimi entity class'ları üzerinden sağlayan property'lerdir.
//Bir propert'nin navigation property olabilmesi için kesinlikle entity türünden olması gerekiyor.
class Calisan //Dependent Entity
{
    public int Id { get; set; }
    public string CalisanAdi { get; set; }
    public int DepartmanId { get; set; } //ForeignKey'e karşılık geliyor.
    public Departman Departman { get; set; } //Calisan entity'si açısından Departman'lara olan Navigation Property'miz. Tekli bir ilişki var. Her bir Calisan'ın bir Departman'ı var.
}
//Burada bire çok bir ilişki var. Çalışanların 1 tane departmanı var. Departmanların birden fazla çalışanları var.
class Departman //Principal Entity
{
    public int Id { get; set; } //Principal Key
    public string DepartmanAdi { get; set; }
    public ICollection<Calisan> Calisanlar { get; set; }
    //Departman'ların birden çok çalışanı var. Yani çoklu ilişki. Bunları da bir Departman için temsil etmek istersek ICollection<Calisan> List türünde Calisan isminde bir 
    //koleksiyon oluşturuyoruz. Departman entity'si açısından Calisan'lara olan Navigation Property'miz.
}

//Navigation property'ler entity'lerdeki tanımlana göre n'e n yahut 1'en şeklinde ilişki türlerini ifade etmektedirler. İleride detaylı!
#endregion

#endregion

#region İlişki Nedir?

#region One to One
//Çalışan ile adresi arasındaki ilişki,
//Eşler arasındaki ilişki gibi.
//Yukarıda eğer Departman'da Calisan'a tekil bir navigation barındırsaydı,
/*
class Departman //Principal Entity
{
    public int Id { get; set; } //Principal Key
    public string DepartmanAdi { get; set; }
    public Calisan Calisanlar { get; set; } //ilişki birebir olacaktı.
}
*/
#endregion

#region One to Many
//Yukarıdaki örnek on to many'e örnektir. Calisan ile Departman arasındaki ilişki.
//Anne-çocuk ilişkisi gibi.
#endregion

#region Many to many
//Kardeşler arasındaki ilişki gibi. 
//Çalışanlar ile projeler arasındaki ilişki.
#endregion
#endregion

#region Entity Framework Core'da İlişki Yapılandırma Yöntemleri

#region Default Conventions
//Varsayılan entity kurallarını kullanarak yapılan ilişki yapılandırma yöntemidir.
//Yukarıda yaptığımız Id'yi otomatik tanıma, Foreign Key'i isminden tanıma vs. Entity'lerin temel kurallarını kullanıyoruz.

//Navigation Property'leri kullanarak ilişki şablonlarını çıkarmaktadır.
#endregion

#region Data Annotations Attributes
//Entity'nin niteliklerine göre ince ayarlar yapmamızı sağlayan attribute'lardır.
// [Key], [ForeignKey]
#endregion

#region FluentAPI
//Entity modellerindeki ilişkileri yapılandırırken daha detaylı çalışmamızı sağlayan öntemdir.
#endregion

#region HasOne
//İlgili entity'nin ilişkisel entity'e birebir ya da bire çok olacak şekilde ilişkisini yapılandırmaya başlayan metottur. 
#endregion

#region HasMany
//İlgili entity'nin ilişkisel entity'e çoka bir ya da çoka çok olacak ilişkisini yapılandırmaya başlayan metottur.
#endregion

#region WithOne
//HasOne ya da HasMany'den sonra birebir ya da çoka bir olacak şekilde ilişki yapılandırmasını tamamlayan metottur.
#endregion

#region WithMany
//HasOne ya da HasMany'den sonra bire çok ya da çoka çok olacak şekilde ilişki yapılandırmasını tamamlayan metottur.
#endregion

#endregion

