using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main(string[] args)
    {
        KatalogProduktow KatalogGlowny = new KatalogProduktow();
        Produkt p1 = new Produkt("Pluszaczek", 60.52, "Zabawki", KatalogGlowny);
        Produkt p2 = new Produkt("Koka Kola", 7.99, "Napoje", KatalogGlowny);
        Produkt p3 = new Produkt("Woda Gazowana", 3.20, "Napoje", KatalogGlowny);
        Produkt p4 = new Produkt("Pierogi", 8.20, "Jedzenie", KatalogGlowny);
        Produkt p5 = new Produkt("Kotek", 150.25, "Zabawki", KatalogGlowny);
        Produkt p6 = new Produkt("Swieczka", 3.40, "Bibeloty", KatalogGlowny);
        Produkt p7 = new Produkt("Durnostójka", 20.20, "Bibeloty", KatalogGlowny);
        Produkt p8 = new Produkt("GównoTurlak", 69.69, "Dla Dorosłych", KatalogGlowny);
        KatalogGlowny.WypiszCalyKatalog();
        KatalogGlowny.WyszukajPoKategorii("Zabawki");
        KatalogGlowny.WypiszCalyKatalog(true);
        KatalogGlowny.WyszukajPoNazwie("no");
        KatalogGlowny.WypiszCalyKatalog(true);
    }
}

// Zaczalem klase Produkt, ale nastepujace rzeczy są temporary do testów:
// TEMP: Konstruktor + Przeciażenie ToString()
// Ustawianie katalogu do ktorego nalezy produkt w konstruktorze to fajne rozwiazanie ktore mozna zostawic
// TODO: w ToString() Dodać wypisywanie ceny w sposob ktory pokazuje tylko 2 cyfry po przecinku
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
        string str = _nazwa + " Cena: " + _cena.ToString() + "zł";
        return str;
    }
}


public class KatalogProduktow
{
    public List<Produkt> lista_produktow = [];

    // lista po filtrowaniu jako zmienna jest po to zeby miec dostep do ostatniego wyniku filtrowania/wyszukiwania
    // po czasie zauwazylem ze nie jest to najmadrzejsze rozwiazanie ale whatever
    public List<Produkt> lista_produktow_po_filtrze = [];
    public List<Produkt> WyszukajPoNazwie(string wyszukiwany_string)
    {
        List<Produkt> przeszukana_lista = [];
        foreach (var _produkt in lista_produktow)
        {
            if (_produkt._nazwa.Contains(wyszukiwany_string)) {przeszukana_lista.Add(_produkt);}
        }
        lista_produktow_po_filtrze = przeszukana_lista;
        return przeszukana_lista;
    }

    // jesli chodzi o kategorie to nwm mozna zrobic enuma, moze to byc poprostu klasa dziedziczna, moze to byc zmienna
    // TODO trzeba ustalic co z ta kategoria
    public List<Produkt> WyszukajPoKategorii(string kategoria)
    {
        List<Produkt> przeszukana_lista = [];
        foreach (var _produkt in lista_produktow)
        {
            if (_produkt._kategoria == kategoria) {przeszukana_lista.Add(_produkt);}
        }
        lista_produktow_po_filtrze = przeszukana_lista;
        return przeszukana_lista;
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