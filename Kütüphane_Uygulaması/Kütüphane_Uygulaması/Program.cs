using System;
using System.Collections.Generic;
using System.IO;


class Kitap
{
    public string ISBN { get; set; }
    public string Ad { get; set; }
    public string Yazar { get; set; }
    public int SayfaSayisi { get; set; }
}


class Kutuphane
{
    List<Kitap> kitaplar = new List<Kitap>();
    string dosyaYolu = "kitaplar.txt";

    public Kutuphane()
    {
        Yükle();
    }


    public void KitapEkle()
    {
        Console.Write("Kitap adı       : ");
        string ad = Console.ReadLine();

        Console.Write("Yazar adı       : ");
        string yazar = Console.ReadLine();

        Console.Write("ISBN            : ");
        string isbn = Console.ReadLine();

        Console.Write("Sayfa sayısı    : ");
        string sayfaStr = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(ad) ||
            string.IsNullOrWhiteSpace(yazar) ||
            string.IsNullOrWhiteSpace(isbn) ||
            string.IsNullOrWhiteSpace(sayfaStr))
        {
            Console.WriteLine("Hata: Hiçbir alan boş bırakılamaz.");
            return;
        }

        
        if (kitaplar.Exists(k => k.ISBN == isbn)) { 
        
            Console.WriteLine("Hata! Bu ISBN'e sahip bir kitap zaten mevcut.");
            return;
        }

        if (!int.TryParse(sayfaStr, out int sayfaSayisi) || sayfaSayisi <= 0)
        {
            Console.WriteLine("Hata! Sayfa sayısı pozitif bir tam sayı olmalıdır.");
            return;
        }

        var kitap = new Kitap
        {
            ISBN = isbn,
            Ad = ad,
            Yazar = yazar,
            SayfaSayisi = sayfaSayisi
        };

        kitaplar.Add(kitap);
        Console.WriteLine("Kitap başarıyla eklendi.");
        Kaydet();
    }

    public void KitaplariListele()
    {
        if (kitaplar.Count == 0)
        {
            Console.WriteLine("Listede hiç kitap yok.");
            return;
        }

        Console.WriteLine("--- Kütüphanedeki Kitaplar ---");
        foreach (var k in kitaplar)
        {
            Console.WriteLine($"ISBN: {k.ISBN} | Ad: {k.Ad} | Yazar: {k.Yazar} | Sayfa: {k.SayfaSayisi}");
        }
    }

    public void KitapAra()
    {
        Console.Write("Aranacak ISBN: ");
        string isbn = Console.ReadLine();

        var kitap = kitaplar.Find(k => k.ISBN == isbn);

        if (kitap == null)
        {
            Console.WriteLine("Kitap bulunamadı.");
            return;
        }
        

        Console.WriteLine($"ISBN: {kitap.ISBN}\nAd: {kitap.Ad}\nYazar: {kitap.Yazar}\nSayfa: {kitap.SayfaSayisi}");
    }

    public void KitapSil()
    {
        Console.Write("Silinecek ISBN: ");
        string isbn = Console.ReadLine();

        var kitap = kitaplar.Find(k => k.ISBN == isbn);

        if (kitap == null)
        {
            Console.WriteLine("Kitap bulunamadı.");
            return;
        }

        kitaplar.Remove(kitap);
        Console.WriteLine("=> Kitap silindi.");
        Kaydet();
    }

   

    public void Yükle()
    {
        try
        {
            if (!File.Exists(dosyaYolu))
            {
                File.Create(dosyaYolu).Close();
                return;
            }

            foreach (var satir in File.ReadAllLines(dosyaYolu))
            {
                var parcalar = satir.Split('|');

                if (int.TryParse(parcalar[3], out int sayfaSayisi))
                {
                    kitaplar.Add(new Kitap
                    {
                        ISBN = parcalar[0],
                        Ad = parcalar[1],
                        Yazar = parcalar[2],
                        SayfaSayisi = sayfaSayisi
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Dosya okunurken hata oluştu: {ex.Message}");
        }
    }

    public void Kaydet()
    {
        try
        {
            var satirlar = new List<string>();
            foreach (var k in kitaplar)
            {
                satirlar.Add($"{k.ISBN}|{k.Ad}|{k.Yazar}|{k.SayfaSayisi}");
            }
            File.WriteAllLines("kitaplar.txt", satirlar);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Dosya yazılırken hata oluştu: {ex.Message}");
        }
    }

   
}


class KutuphaneProgram
{
    static void Main(string[] args)
    {
        Kutuphane kutuphane = new Kutuphane();
        bool devam = true;

        while (devam)
        {
            Console.WriteLine("\n=== Kütüphane Uygulaması ===");
            Console.WriteLine("1. Kitap Ekle");
            Console.WriteLine("2. Kitapları Listele");
            Console.WriteLine("3. Kitap Ara (ISBN)");
            Console.WriteLine("4. Kitap Sil (ISBN)");
            Console.WriteLine("5. Kaydet ve Çık");
            Console.Write("Seçiminiz: ");

            string secim = Console.ReadLine();
            Console.WriteLine();

            switch (secim)
            {
                case "1":
                    kutuphane.KitapEkle();
                    break;
                case "2":
                    kutuphane.KitaplariListele();
                    break;
                case "3":
                    kutuphane.KitapAra();
                    break;
                case "4":
                    kutuphane.KitapSil();
                    break;
                case "5":
                    kutuphane.Kaydet();
                    devam = false;
                    break;
                default:
                    Console.WriteLine("Geçersiz seçim yaptınız.");
                    break;
            }
        }
    }
}