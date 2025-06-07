using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        KatalogProduktow KatalogGlowny = new KatalogProduktow();
        Produkt p1 = new Produkt("Pluszaczek", 60.52, "Zabawki", KatalogGlowny);
        Produkt p2 = new Napoje(true,"plastik","06.06.2030","Koka Kola", 7.99, "Napoje", KatalogGlowny);
        Produkt p3 = new Produkt("Woda Gazowana", 3.20, "Napoje", KatalogGlowny);
        Produkt p4 = new Jedzenie(false,true,"09.09.2031","Pierogi", 8.20, "Jedzenie", KatalogGlowny);
        Produkt p5 = new Produkt("Kotek", 150.25, "Zabawki", KatalogGlowny);
        Produkt p6 = new Produkt("Swieczka", 3.40, "Bibeloty", KatalogGlowny);
        Produkt p7 = new Produkt("Durnostójka", 20.20, "Bibeloty", KatalogGlowny);
        Produkt p8 = new Produkt("GównoTurlak", 69.69, "Dla Dorosłych", KatalogGlowny);
        KatalogGlowny.WypiszCalyKatalog();
        KatalogGlowny.WyszukajPoKategorii("Zabawki");
        KatalogGlowny.WypiszCalyKatalog(true);
        KatalogGlowny.WyszukajPoNazwie("no");
        KatalogGlowny.WypiszCalyKatalog(true);

        LoginRej loginRej = new LoginRej();

        loginRej.Rejestracja("admin", "Aa1!");
        loginRej.Logowanie("adam", "15");
        loginRej.Logowanie("admin", "Aa1!");

        KontoUzytkownika admin = loginRej.GetKonto("admin");
        admin.Info();
        loginRej.ZmianaHasla("admin","Bb1!");
        admin.Info();
        loginRej.ZmianaNazwy("admin", "GIGAdmin");
        admin.Info();

        admin.KoszykProduktow.DodajDoKoszyka(p1);
        admin.KoszykProduktow.DodajDoKoszyka(p1);
        admin.KoszykProduktow.DodajDoKoszyka(p1);
        admin.KoszykProduktow.DodajDoKoszyka(p2);
        admin.KoszykProduktow.DodajDoKoszyka(p3);
        admin.KoszykProduktow.DodajDoKoszyka(p3);

        admin.KoszykProduktow.WyswietlKoszyk();

        admin.KoszykProduktow.UsunZKoszyka(p3);

        admin.KoszykProduktow.WyswietlKoszyk();

    }
}

// Zaczalem klase Produkt, ale nastepujace rzeczy są temporary do testów:
// TEMP: Konstruktor + Przeciażenie ToString()
// Ustawianie katalogu do ktorego nalezy produkt w konstruktorze to fajne rozwiazanie ktore mozna zostawic
public class Produkt
{
    public string _nazwa;
    public double _cena;
    public string _kategoria;
    public Produkt(string nazwa, double cena, string kategoria, KatalogProduktow katalog) 
    {
        _nazwa = nazwa;
        _cena = cena;
        _kategoria = kategoria;
        katalog.DodajProdukt(this);
    }
    public override string ToString()
    {
        string str = _nazwa + " Cena: " + _cena.ToString("0.00") + "zł";
        return str;
    }
}
// Klasy Dziciczne
public class Zywnosc : Produkt{
    public string _termin;
    public Zywnosc(string termin,string nazwa, double cena, string kategoria, KatalogProduktow katalog):base(nazwa,cena,kategoria,katalog){
        _termin=termin;
    }
}

public class Napoje : Zywnosc{
    public string _rodzaj_opakowania;
    public bool _gazowane;
    public Napoje(bool gazowane,string rodzaj_opakowania,string termin,string nazwa, double cena, string kategoria, KatalogProduktow katalog):base(termin,nazwa,cena,kategoria,katalog){
        _rodzaj_opakowania=rodzaj_opakowania;
        _gazowane=gazowane;
    }
}

public class Jedzenie : Zywnosc{
    public bool _organiczne;
    public bool _bezglutenowe;
    public Jedzenie(bool organiczne,bool bezglutenowe,string termin,string nazwa, double cena, string kategoria, KatalogProduktow katalog):base(termin,nazwa,cena,kategoria,katalog){
        _organiczne=organiczne;
        _bezglutenowe=bezglutenowe;
    }
}

public class Elektronika : Produkt{
    public float _zuzycie_energii;
    public Elektronika(float zuzycie_energii,string nazwa, double cena, string kategoria, KatalogProduktow katalog):base(nazwa,cena,kategoria,katalog){
        _zuzycie_energii=zuzycie_energii;
    }
}

public class Zegary : Elektronika{
    public string _rodzaj;
    public Zegary(string rodzaj,float zuzycie_energii,string nazwa, double cena, string kategoria, KatalogProduktow katalog):base(zuzycie_energii,nazwa,cena,kategoria,katalog){
        _rodzaj=rodzaj;
    }
}

public class Telewizory : Elektronika{
    public float _ile_cali;
    public Telewizory(float ile_cali,float zuzycie_energii,string nazwa, double cena, string kategoria, KatalogProduktow katalog):base(zuzycie_energii,nazwa,cena,kategoria,katalog){
        _ile_cali=ile_cali;
    }
}

//Katalog Produktow
public class KatalogProduktow
{
    public List<Produkt> lista_produktow = [];

    // lista po filtrowaniu jako zmienna jest po to zeby miec dostep do ostatniego wyniku filtrowania/wyszukiwania
    // po czasie zauwazylem ze nie jest to najmadrzejsze rozwiazanie ale whatever
    // zosia: si było niepotzrbne powtorzenie z ta lista produktow po filtrze = lista przeszukana czy tam na odwrot
    public List<Produkt> lista_produktow_po_filtrze = [];
    public List<Produkt> WyszukajPoNazwie(string wyszukiwany_string)
    {
        lista_produktow_po_filtrze.Clear();
        foreach (var _produkt in lista_produktow)
        {
            if (_produkt._nazwa.Contains(wyszukiwany_string)) {lista_produktow_po_filtrze.Add(_produkt);}
        }
        return lista_produktow_po_filtrze;
    }

    // jesli chodzi o kategorie to nwm mozna zrobic enuma, moze to byc poprostu klasa dziedziczna, moze to byc zmienna
    // TODO trzeba ustalic co z ta kategoria
    public List<Produkt> WyszukajPoKategorii(string kategoria)
    {
        lista_produktow_po_filtrze.Clear();
        foreach (var _produkt in lista_produktow)
        {
            if (_produkt._kategoria == kategoria) {lista_produktow_po_filtrze.Add(_produkt);}
        }
        return lista_produktow_po_filtrze;
    }

    // intuicyjne mam nadzieje. Mozna ustalic przy wywolaniu czy chcemy wypisac tylko
    // wynik ostatniego filtrowania, czy caly katalog
    public void WypiszCalyKatalog(bool wypisz_filtrowana = false)
    {
        List<Produkt> lista_do_wypisania = [];
        if (wypisz_filtrowana) {lista_do_wypisania = lista_produktow_po_filtrze;}
        else {lista_do_wypisania = lista_produktow;}
        Console.WriteLine(lista_do_wypisania.Count().ToString() + " Produktów!");
        foreach (var _produkt in lista_do_wypisania)
        {
            Console.WriteLine(_produkt);
        }
        Console.WriteLine("\n");
    }

    public void DodajProdukt(Produkt _produkt)
    {
        lista_produktow.Add(_produkt);
    }
}

//Konto Uzytkownika

public class KontoUzytkownika
{
    private string nazwaKonta;
    private string haslo;
    private Koszyk koszykProduktow;
    // historia zamowien
    
    public string NazwaKonta
    {
        get { return nazwaKonta; }
        set { nazwaKonta = value; }
    }

    public string Haslo
    {
        get { return haslo; }
        set { haslo = value; }
    }

    public Koszyk KoszykProduktow
    {
        get { return koszykProduktow; }
        set { koszykProduktow = value; }
    }

    public KontoUzytkownika (string nazwaKonta, string haslo)
    {
        NazwaKonta = nazwaKonta;
        Haslo = haslo;
        KoszykProduktow = new Koszyk();
    }

    public void Info()
    {
        Console.WriteLine($"nazwa: {NazwaKonta}\nhaslo: {Haslo}");
    }

}

//Logowanie i Rejestracja

public class LoginRej
{
    private List<KontoUzytkownika> uzytkownicy = new List<KontoUzytkownika>();

    public bool WalidacjaHasla(string haslo)
    {
        int[] walidacjaHasla = [0, 0, 0, 0];
        int licznik = 0;

        foreach (char c in haslo)
        {
            int hAscii = (int)c;

            if (hAscii > 31 && hAscii < 128)
            {
                switch (hAscii)
                {
                    case int n when n >= 48 && n <= 57: //cyfry
                        walidacjaHasla[0]++;
                        break;
                    case int n when n >= 65 && n <= 90: //duze litery
                        walidacjaHasla[1]++;
                        break;
                    case int n when n >= 97 && n <= 122: //male litery
                        walidacjaHasla[2]++;
                        break;
                    default: //znaki specjalne
                        walidacjaHasla[3]++;
                        break;
                }
            }
        }

        foreach (int i in walidacjaHasla)
        {
            if (i > 0)
            {
                licznik++;
            }
        }

        if (licznik == 4)
        {
            return true;
        }
        else
        {
            Console.WriteLine("Haslo musi zawierac: male lietry, wielkie lietry, cyfry, zanki specjalne");
            return false;
        }
    }


    public bool Rejestracja(string nazwaKonta, string haslo)
    {
        if (uzytkownicy.Any(u => u.NazwaKonta == nazwaKonta))
        {
            Console.WriteLine("Taka nazwa uzytkownika już istnieje");
            return false;
        }
        else
        {
            if (WalidacjaHasla(haslo))
            {
                uzytkownicy.Add(new KontoUzytkownika(nazwaKonta, haslo));
                Console.WriteLine("Konto zostało utowzone");
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool Logowanie(string nazwaKonta, string haslo)
    {
        if (uzytkownicy.Any(u => u.NazwaKonta == nazwaKonta && u.Haslo == haslo))
        {
            Console.WriteLine("zalogowano");
            return true;
        }
        else
        {
            Console.WriteLine("logowanie nie udane");
            return false;
        }
    }

    public KontoUzytkownika GetKonto (string nazwaKonta)
    {
        return uzytkownicy.FirstOrDefault(u => u.NazwaKonta == nazwaKonta);
    }

    public bool ZmianaNazwy(string staraNazwa, string nowaNazwa)
    {
        KontoUzytkownika uzytkownik = GetKonto(staraNazwa);

        if (staraNazwa == nowaNazwa)
        {
            Console.WriteLine("Nowa nazwa nie może być taka sama jak poprzednia");
            return false;
        }

        if (uzytkownicy.Any(u => u.NazwaKonta == nowaNazwa))
        {
            Console.WriteLine("Taki użytkownik już istnieje");
            return false;
        }

        if (uzytkownik != null)
        {
            uzytkownik.NazwaKonta = nowaNazwa;
            Console.WriteLine("Nazwa zostala zmieniona");
            return true;
        }

        Console.WriteLine("Zmiana sie niepowiodla");
        return false;
    }

    public bool ZmianaHasla(string nazwaKonta,string noweHaslo)
    {
        KontoUzytkownika uzytkownik = GetKonto(nazwaKonta);

        if (uzytkownik.Haslo == noweHaslo)
        {
            Console.WriteLine("Nowe haslo nie moze byc takie samo jak poprzednie");
            return false;
        }

        if (WalidacjaHasla(noweHaslo))
        {
            uzytkownik.Haslo = noweHaslo;
            Console.WriteLine("Haslo zostalo zmienione");
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class Koszyk
{
    private List<Produkt> produkty = new List<Produkt>();

    public void DodajDoKoszyka(Produkt produkt)
    {
        produkty.Add(produkt);
    }

    public void UsunZKoszyka(Produkt produkt)
    {
        produkty.Remove(produkt);
    }

    public double Suma()
    {
        double suma = 0;

        foreach(var i in produkty)
        {
            suma += i._cena;
        }

        return suma;
    }

    public void WyswietlKoszyk()
    {
        var pogrupowane = produkty
            .GroupBy(p => p)
            .Select(g => new { Produkt = g.Key, Ilosc = g.Count() });
        
        foreach (var i in pogrupowane)
        {
            Console.WriteLine($"{i.Produkt._nazwa} - {i.Ilosc} - {i.Produkt._cena * i.Ilosc}");
        }

        Console.WriteLine($"łączna kwota to {Suma()}");
    }
}
