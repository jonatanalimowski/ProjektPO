using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        LoginRej loginRej = new LoginRej();

        loginRej.Rejestracja("Admin", "Aa1!", true);
        loginRej.Rejestracja("gosc", "Aa1!", false);
        Console.Clear();

        KatalogProduktow KatalogGlowny = new KatalogProduktow();

        Produkt p1 = new Produkt("Pluszaczek", 60.52f, "Zabawki", KatalogGlowny, 50);
        Produkt p2 = new Produkt("Kotek", 150.25f, "Zabawki", KatalogGlowny, 50);
        Produkt p3 = new Napoje(true, "puszka", "06.06.2030", "Koka Kola", 7.99f, "Napoje", KatalogGlowny, 50);
        Produkt p4 = new Napoje(true, "plastik", "23.02.2029" ,"Woda Gazowana", 3.20f, "Napoje", KatalogGlowny ,300); 
        Produkt p5 = new Jedzenie(false, true, "09.09.2031", "Pierogi", 8.20f, "Jedzenie", KatalogGlowny, 50);
        Produkt p6 = new Jedzenie(true, false, "12.06.2026", "Chleb", 4.19f, "Jedzenie", KatalogGlowny, 60);
        Produkt p7 = new Telewizory(33.25f, 3.3f, "Samrt TV", 1775.5f, "Telewizory", KatalogGlowny, 30);
        Produkt p8 = new Telewizory(60.5f, 5.57f, "ULTRA Samrt TV", 3857.99f, "Telewizory", KatalogGlowny, 30);
        Produkt p9 = new Zegary("Z Kukulka", 0.3f, "Zegar Vintage Z Kukulka", 90.5f, "Zegary", KatalogGlowny, 15);
        Produkt p10 = new Zegary("Elektorniczy", 0.1f, "Zegar Elektorniczny", 29.99f, "Zegary", KatalogGlowny, 100);

        Interfejs ui = new Interfejs(loginRej);

        string konto = "";
        while (konto != "3")
        {
            konto = ui.LogowanieDoSystemu();
            if(konto == "3")
            {
                Environment.Exit(0);
            }
            ui.Operacje(konto, KatalogGlowny, loginRej);
        }

    }
}

public class Produkt
{
    public string _nazwa;
    public float _cena;
    public string _kategoria;
    public int _dostepnosc;
    public Produkt(string nazwa, float cena, string kategoria, KatalogProduktow katalog, int dostepnosc) 
    {
        _nazwa = nazwa;
        _cena = cena;
        _kategoria = kategoria;
        _dostepnosc= dostepnosc;
        katalog.DodajProdukt(this);
    }
    public override string ToString()
    {
        string str = _nazwa + " Cena: " + _cena.ToString("0.00") + "zł " + "Dostepność: " + _dostepnosc;
        return str;
    }
}

public class Zywnosc : Produkt{
    public string _termin;
    public Zywnosc(string termin,string nazwa, float cena, string kategoria, KatalogProduktow katalog, int dostepnosc):base(nazwa,cena,kategoria,katalog,dostepnosc)
    {
        _termin=termin;
    }
}

public class Napoje : Zywnosc{
    public string _rodzaj_opakowania;
    public bool _gazowane;
    public Napoje(bool gazowane,string rodzaj_opakowania,string termin,string nazwa, float cena, string kategoria, KatalogProduktow katalog, int dostepnosc):base(termin,nazwa,cena,kategoria,katalog,dostepnosc){
        _rodzaj_opakowania=rodzaj_opakowania;
        _gazowane=gazowane;
    }
}

public class Jedzenie : Zywnosc{
    public bool _organiczne;
    public bool _bezglutenowe;
    public Jedzenie(bool organiczne,bool bezglutenowe,string termin,string nazwa, float cena, string kategoria, KatalogProduktow katalog, int dostepnosc) :base(termin,nazwa,cena,kategoria,katalog,dostepnosc)
    {
        _organiczne=organiczne;
        _bezglutenowe=bezglutenowe;
    }
}

public class Elektronika : Produkt{
    public float _zuzycie_energii;
    public Elektronika(float zuzycie_energii,string nazwa, float cena, string kategoria, KatalogProduktow katalog, int dostepnosc) :base(nazwa,cena,kategoria,katalog,dostepnosc){
        _zuzycie_energii=zuzycie_energii;
    }
}

public class Zegary : Elektronika{
    public string _rodzaj;
    public Zegary(string rodzaj,float zuzycie_energii,string nazwa, float cena, string kategoria, KatalogProduktow katalog, int dostepnosc) :base(zuzycie_energii,nazwa,cena,kategoria,katalog, dostepnosc)
    {
        _rodzaj=rodzaj;
    }
}

public class Telewizory : Elektronika{
    public float _ile_cali;
    public Telewizory(float ile_cali,float zuzycie_energii,string nazwa, float cena, string kategoria, KatalogProduktow katalog, int dostepnosc) :base(zuzycie_energii,nazwa,cena,kategoria,katalog,dostepnosc)
    {
        _ile_cali=ile_cali;
    }
}


public class KatalogProduktow
{
    public List<Produkt> lista_produktow = [];

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

    public List<Produkt> WyszukajPoKategorii(string kategoria)
    {
        lista_produktow_po_filtrze.Clear();
        foreach (var _produkt in lista_produktow)
        {
            if (_produkt._kategoria == kategoria) {lista_produktow_po_filtrze.Add(_produkt);}
        }
        return lista_produktow_po_filtrze;
    }

    public void WypiszCalyKatalog(bool wypisz_filtrowana = false)
    {
        List<Produkt> lista_do_wypisania = [];
        if (wypisz_filtrowana) {lista_do_wypisania = lista_produktow_po_filtrze;}
        else {lista_do_wypisania = lista_produktow;}
        Console.WriteLine(lista_do_wypisania.Count().ToString() + " Produktow!");
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

    public void UsunZKatalogu(Produkt produkt)
    {
        lista_produktow.Remove(produkt);
    }

}

public class KontoUzytkownika
{
    private string nazwaKonta;
    private string haslo;
    private Koszyk koszykProduktow;
    private List<Zamowienie> historiaZamowien;
    private bool typ; //false - uzytkownik true - admin

    
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

    public bool Typ
    {
        get { return typ; }
        set { typ = value; }
    }

    public KontoUzytkownika (string nazwaKonta, string haslo, bool typ)
    {
        NazwaKonta = nazwaKonta;
        Haslo = haslo;
        KoszykProduktow = new Koszyk();
        historiaZamowien = new List<Zamowienie>();
        Typ = typ;
    }

    public void Info()
    {
        Console.WriteLine($"Nazwa: {NazwaKonta}\nHaslo: {Haslo}");
    }

    public void DodajZamowienie(Zamowienie a){
        historiaZamowien.Add(a);
    }
    public string WyswietlHistorie()
    {
        string historia="";
        int licznik = 1;
        foreach(var i in historiaZamowien){
            historia=$"{historia}\n{licznik}. {i.GenerujPodsumowanie()} \n";
            licznik++;
        }
        return historia;
    }

    public void ZakupProdukty(){
        string wybor="";
        List<Produkt> zamownie = koszykProduktow.PrzekazListy();
        Console.WriteLine("Czy dokonac zakupu? - tak lub nie: ");
        wybor=Console.ReadLine();
        if(wybor.ToLower()=="tak"){
            if (zamownie.Count() != 0)
            {
                DodajZamowienie(new Zamowienie(koszykProduktow.PrzekazListy(), (float)koszykProduktow.Suma(), "złozne"));
                foreach (var i in koszykProduktow.PrzekazListy())
                {
                    i._dostepnosc = i._dostepnosc - 1;
                }
                koszykProduktow.WyczyscKoszyk();
            }
            else
            {
                Console.WriteLine("Koszyk jest pusty");
            }
        }else{
            Console.WriteLine("Przerwano proces wykonywania zakupu");
        }

    }

}

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


    public bool Rejestracja(string nazwaKonta, string haslo, bool typ)
    {
        if (uzytkownicy.Any(u => u.NazwaKonta == nazwaKonta))
        {
            Console.WriteLine("Taka nazwa uzytkownika juz istnieje");
            return false;
        }
        else
        {
            if (WalidacjaHasla(haslo))
            {
                uzytkownicy.Add(new KontoUzytkownika(nazwaKonta, haslo, typ));
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
            Console.WriteLine("Zalogowano");
            return true;
        }
        else
        {
            Console.WriteLine("Logowanie nie udane");
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
            Console.WriteLine("Nowa nazwa nie moze byc taka sama jak poprzednia");
            return false;
        }

        if (uzytkownicy.Any(u => u.NazwaKonta == nowaNazwa))
        {
            Console.WriteLine("Taki uzytkownik juz istnieje");
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
    public List<Produkt> lista_produktow_po_filtrze = [];

    public void DodajDoKoszyka(Produkt produkt, int ilosc)
    {
        if (produkt._dostepnosc < ilosc)
        {
            Console.WriteLine("Nie mamy tyle produktu w magazynie");
        }
        else
        {
            for (int i = 0; i < ilosc; i++)
            {
                produkty.Add(produkt);
            }
        }
    }

    public void UsunZKoszyka(Produkt produkt)
    {
        int ilosc_max = 0;
        int operacja = 0;

        Console.WriteLine("Usun wszystkie wystapienia produkut (1)/ Usun konkretna ilosc (2): ");
        operacja = Convert.ToInt32(Console.ReadLine());

        var pogrupowane = produkty
            .GroupBy(p => p)
            .Select(g => new { Produkt = g.Key, Ilosc = g.Count() });

        foreach (var i in pogrupowane)
        {
            if (i.Produkt._nazwa == produkt._nazwa)
            {
                ilosc_max = i.Ilosc;
            }
        }

        switch (operacja)
        {
            case 1:
                for (int i = 0; i < ilosc_max; i++)
                {
                    produkty.Remove(produkt);
                }
                break;
            case 2:
                int ilosc = 0;

                Console.WriteLine("Podaj ile produktow chcesz usunac: ");
                ilosc = Convert.ToInt32(Console.ReadLine());

                if (ilosc <= ilosc_max)
                {
                    for (int i = 0; i < ilosc; i++)
                    {
                        produkty.Remove(produkt);
                    }
                }
                else
                {
                    Console.WriteLine("Nie odpowiednia ilosc");
                }
                break;
            default:
                Console.WriteLine("Nie odpowidnia operajca");
                break;
        }

        produkty.Remove(produkt);
    }

    public List<Produkt> WyszukajPoNazwie(string wyszukiwany_string)
    {
        lista_produktow_po_filtrze.Clear();
        foreach (var produkt in produkty)
        {
            if (produkt._nazwa.Contains(wyszukiwany_string)) { lista_produktow_po_filtrze.Add(produkt); }
        }
        return lista_produktow_po_filtrze;
    }

    public void WyczyscKoszyk(){
        produkty.Clear();
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

    public string WyswietlKoszyk()
    {
        string lista="";
        var pogrupowane = produkty
            .GroupBy(p => p)
            .Select(g => new { Produkt = g.Key, Ilosc = g.Count() });
        
        foreach (var i in pogrupowane)
        {
            lista=lista + $"{i.Produkt._nazwa} - {i.Ilosc} - {i.Produkt._cena * i.Ilosc}"+"\n";
        }

        return lista + $"łączna kwota to {Suma().ToString("0.00")}";
    }

    public List<Produkt> PrzekazListy(){
        return produkty;
    }
}

public class Zamowienie
{
    private List<Produkt> _produkty;
    
    private float _cena_laczna;
    private string _status;

    public Zamowienie(List<Produkt> produkty,float cena_laczna,string status)
    {
    _produkty = new List<Produkt>(produkty); 
    _cena_laczna = cena_laczna;
    _status = status;
    } 
    public string GenerujPodsumowanie()
    {
        string produktywypis="";

        var pogrupowane = _produkty
            .GroupBy(p => p)
            .Select(g => new { Prod = g.Key, Ilosc = g.Count() });

        foreach(var i in pogrupowane)
        {
            produktywypis = produktywypis + $"{i.Prod._nazwa} - {i.Ilosc} - {i.Prod._cena * i.Ilosc}"+"\n";
        }
        return $"\nStan zamowienia: {_status}\nKupiono:\n{produktywypis}Zapłacono razem: {_cena_laczna}zł";
    }
    
}
    
public class Interfejs
{
    private LoginRej zarzadKont;

    public LoginRej ZarzadKont
    {
        get { return zarzadKont; }
        set { zarzadKont = value; }
    }

    public Interfejs(LoginRej zarzadKont)
    {
        ZarzadKont = zarzadKont;
    }

    public string LogowanieDoSystemu()
    {
        int logCzyRej = 0;
        string nazwa = "";
        string haslo = "";
        bool sukces = false;

        Console.WriteLine("Witamy W Sklepie Intertowy!!!\n");
        Console.WriteLine("Zaloguj sie (1)/ Zarejstruj sie (2)/ Wyjdz (3): \n");
        

        while (!sukces)
        {
            logCzyRej = Convert.ToInt32(Console.ReadLine());

            switch (logCzyRej)
            {
                case 1:
                    while (!sukces)
                    {
                        Console.WriteLine("Login: ");
                        nazwa = Console.ReadLine();
                        Console.WriteLine("Haslo: ");
                        haslo = Console.ReadLine();

                        sukces = ZarzadKont.Logowanie(nazwa, haslo);
                    }
                    break;
                case 2:
                    while (!sukces)
                    {
                        Console.WriteLine("Login: ");
                        nazwa = Console.ReadLine();
                        Console.WriteLine("Haslo: ");
                        haslo = Console.ReadLine();

                        sukces = ZarzadKont.Rejestracja(nazwa, haslo, false);
                    }
                    break;
                case 3:
                    return "3";
                    break;
                default:
                    Console.WriteLine("Niepoprawna opercja, srpobuj ponownie");
                    break;
            }
        }

        return nazwa;
    }

    public void Operacje(string nazwa, KatalogProduktow katalog, LoginRej zarzadanie)
    {
        KontoUzytkownika uzytkownik = ZarzadKont.GetKonto(nazwa);
        int operacja = 0;
        bool zamkij = false;

        if (!uzytkownik.Typ)
        {
            while (!zamkij)
            {
                Console.WriteLine("Jakiej operacji chcesz dokonac");
                Console.WriteLine("1 - Przejrzyj Katalog");
                Console.WriteLine("2 - Dodaj do koszyka");
                Console.WriteLine("3 - Usun z koszyka");
                Console.WriteLine("4 - Przejrzyj ksozyk");
                Console.WriteLine("5 - Zloz zamowienie");
                Console.WriteLine("6 - Historai Zamowien");
                Console.WriteLine("7 - Wyloguj sie");

                operacja = Convert.ToInt32(Console.ReadLine());
                Console.Clear();

                switch (operacja)
                {
                    case 1:
                        string filtr = "";
                        int opcja = 0;

                        Console.WriteLine("Wyswietl wszsytko (1)/ Szukaj po nazwie (2)/ Szukaj po kategorii (3): ");
                        opcja = Convert.ToInt32(Console.ReadLine());

                        switch (opcja)
                        {
                            case 1:
                                katalog.WypiszCalyKatalog();
                                break;
                            case 2:
                                Console.WriteLine("Wyszukaj: ");
                                filtr = Console.ReadLine();

                                katalog.WyszukajPoNazwie(filtr);
                                katalog.WypiszCalyKatalog(true);
                                break;
                            case 3:
                                Console.WriteLine("Wyszukaj: ");
                                filtr = Console.ReadLine();

                                katalog.WyszukajPoKategorii(filtr);
                                katalog.WypiszCalyKatalog(true);
                                break;
                            default:
                                Console.WriteLine("Niepoprawna opercja, srpobuj ponownie");
                                break;
                        }
                        break;
                    case 2:
                        List<Produkt> szukany_add = new List<Produkt>();
                        string prod_add = "";
                        int prod_il_add = 0;

                        while (szukany_add.Count == 0)
                        {
                            Console.WriteLine("Podaj nazwe produktu: ");
                            prod_add = Console.ReadLine();

                            szukany_add = katalog.WyszukajPoNazwie(prod_add);
                            if (szukany_add.Count == 0)
                            {
                                Console.WriteLine("Zla nazwa, sporbuj ponownie");
                            }
                        }

                        Console.WriteLine("Podaj ilosc produktu: ");
                        prod_il_add = Convert.ToInt32(Console.ReadLine());


                        uzytkownik.KoszykProduktow.DodajDoKoszyka(szukany_add[0], prod_il_add);
                        szukany_add.Clear();
                        break;
                    case 3:
                        List<Produkt> szukany_del = new List<Produkt>();
                        string prod_del = "";

                        while (szukany_del.Count == 0)
                        {
                            Console.WriteLine("Podaj nazwe produktu: ");
                            prod_del = Console.ReadLine();

                            szukany_del = uzytkownik.KoszykProduktow.WyszukajPoNazwie(prod_del);
                            if (szukany_del.Count == 0)
                            {
                                Console.WriteLine("Zla nazwa, sporbuj ponownie");
                            }
                        }

                        uzytkownik.KoszykProduktow.UsunZKoszyka(szukany_del[0]);
                        szukany_del.Clear();
                        break;
                    case 4:
                        Console.WriteLine(uzytkownik.KoszykProduktow.WyswietlKoszyk());
                        break;
                    case 5:
                        uzytkownik.ZakupProdukty();
                        break;
                    case 6:
                        Console.WriteLine(uzytkownik.WyswietlHistorie());
                        break;
                    case 7:
                        Console.WriteLine("Do zobaczenia!");
                        zamkij = true;
                        break;
                    default:
                        Console.WriteLine("Niepoprawna opercja, srpobuj ponownie");
                        break;
                }
            }

        }
        else
        {
            while (!zamkij)
            {
                Console.WriteLine("Jakiej operacji chcesz dokonac");
                Console.WriteLine("1 - Przejrzyj Katalog");
                Console.WriteLine("2 - Dodaj do katalogu");
                Console.WriteLine("3 - Usun z katalogu");
                Console.WriteLine("4 - Zmien dostepnosc");
                Console.WriteLine("5 - Stworz konto adminstratora");
                Console.WriteLine("6 - Wyloguj sie");

                operacja = Convert.ToInt32(Console.ReadLine());
                Console.Clear();

                switch (operacja)
                {
                    case 1:
                        string filtr = "";
                        int opcja = 0;

                        Console.WriteLine("Wyswietl wszsytko (1)/ Szukaj po nazwie (2)/ Szukaj po kategorii (3): ");
                        opcja = Convert.ToInt32(Console.ReadLine());

                        switch (opcja)
                        {
                            case 1:
                                katalog.WypiszCalyKatalog();
                                break;
                            case 2:
                                Console.WriteLine("Wyszukaj: ");
                                filtr = Console.ReadLine();

                                katalog.WyszukajPoNazwie(filtr);
                                katalog.WypiszCalyKatalog(true);
                                break;
                            case 3:
                                Console.WriteLine("Wyszukaj: ");
                                filtr = Console.ReadLine();

                                katalog.WyszukajPoKategorii(filtr);
                                katalog.WypiszCalyKatalog(true);
                                break;
                            default:
                                Console.WriteLine("Niepoprawna opercja, srpobuj ponownie");
                                break;
                        }
                        break;
                    case 2:
                        int kategoria = 0;

                        Console.WriteLine("Wybierz kategorie");
                        Console.WriteLine("1 - Produkt bazowy");
                        Console.WriteLine("2 - Napoje");
                        Console.WriteLine("3 - Jedzenie");
                        Console.WriteLine("4 - Zegary");
                        Console.WriteLine("5 - Telewizory");
                        kategoria = Convert.ToInt32(Console.ReadLine());

                        string nazwa_prod = "";
                        float cena = 0;
                        int dosteponosc = 0;
                        string termin = "";
                        float zuzycieEnergi = 0;

                        switch (kategoria)
                        {
                            case 1:
                                Console.WriteLine("Podaj nazwe: ");
                                nazwa_prod = Console.ReadLine();
                                Console.WriteLine("Podaj cene: ");
                                cena = Convert.ToSingle(Console.ReadLine());
                                Console.WriteLine("Podaj dostponosc: ");
                                dosteponosc = Convert.ToInt32(Console.ReadLine());

                                new Produkt(nazwa_prod, cena, "Produkt", katalog, dosteponosc);
                                break;
                            case 2:
                                string rodzajOpakowania = "";
                                bool gazowane = false;

                                Console.WriteLine("Podaj nazwe: ");
                                nazwa_prod = Console.ReadLine();
                                Console.WriteLine("Podaj cene: ");
                                cena = Convert.ToSingle(Console.ReadLine());
                                Console.WriteLine("Podaj dostponosc: ");
                                dosteponosc = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Podaj date waznosci: ");
                                termin = Console.ReadLine();
                                Console.WriteLine("Podaj rodzaj opakowania: ");
                                rodzajOpakowania = Console.ReadLine();
                                Console.WriteLine("Podaj czy gazowane (0 - nie/ 1- tak): ");
                                string pom = Console.ReadLine();

                                if (pom == "1")
                                {
                                    gazowane = true;
                                }

                                new Napoje(gazowane, rodzajOpakowania, termin, nazwa_prod, cena, "Napoje", katalog, dosteponosc);
                                break;
                            case 3:
                                bool organiczne = false;
                                bool bezglutenowe = false;

                                Console.WriteLine("Podaj nazwe: ");
                                nazwa_prod = Console.ReadLine();
                                Console.WriteLine("Podaj cene: ");
                                cena = Convert.ToSingle(Console.ReadLine());
                                Console.WriteLine("Podaj dostponosc: ");
                                dosteponosc = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Podaj date waznosci: ");
                                termin = Console.ReadLine();
                                Console.WriteLine("Podaj czy organiczne (0 - nie/ 1- tak): ");
                                string pom_o = Console.ReadLine();
                                Console.WriteLine("Podaj czy bezglutenowe (0 - nie/ 1- tak): ");
                                string pom_g = Console.ReadLine();

                                if (pom_o == "1")
                                {
                                    organiczne = true;
                                }

                                if (pom_g == "1")
                                {
                                    bezglutenowe = true;
                                }

                                new Jedzenie(organiczne, bezglutenowe, termin, nazwa_prod, cena, "Jedzenie", katalog, dosteponosc);
                                break;
                            case 4:
                                string rodzaj = "";

                                Console.WriteLine("Podaj nazwe: ");
                                nazwa_prod = Console.ReadLine();
                                Console.WriteLine("Podaj cene: ");
                                cena = Convert.ToSingle(Console.ReadLine());
                                Console.WriteLine("Podaj dostponosc: ");
                                dosteponosc = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Podaj zuzycie energii: ");
                                termin = Console.ReadLine();
                                Console.WriteLine("Podaj rodzaj: ");
                                rodzaj = Console.ReadLine();

                                new Zegary(rodzaj, zuzycieEnergi, nazwa, cena, "Zegary", katalog, dosteponosc);
                                break;
                            case 5:
                                float ileCali = 0;

                                Console.WriteLine("Podaj nazwe: ");
                                nazwa_prod = Console.ReadLine();
                                Console.WriteLine("Podaj cene: ");
                                cena = Convert.ToSingle(Console.ReadLine());
                                Console.WriteLine("Podaj dostponosc: ");
                                dosteponosc = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Podaj zuzycie energii: ");
                                termin = Console.ReadLine();
                                Console.WriteLine("Podaj ilosc cali: ");
                                ileCali = Convert.ToSingle(Console.ReadLine());

                                new Telewizory(ileCali, zuzycieEnergi, nazwa, cena, "Telewizory", katalog, dosteponosc);
                                break;
                            default:
                                Console.WriteLine("Niepoprawna opercja, srpobuj ponownie");
                                break;
                        }
                        break;
                    case 3:
                        string nazwaProduktuDel = "";
                        List<Produkt> porduktDel = [];

                        Console.WriteLine("Podaj nazwe produkut: ");
                        nazwaProduktuDel = Console.ReadLine();
                        porduktDel = katalog.WyszukajPoNazwie(nazwaProduktuDel);
                        katalog.UsunZKatalogu(porduktDel[0]);
                        break; ;
                    case 4:
                        string nazwaProduktuZD = "";
                        List<Produkt> porduktZD = [];
                        int nowaIlosc = 0;

                        Console.WriteLine("Podaj nazwe produkut: ");
                        nazwaProduktuZD = Console.ReadLine();
                        porduktZD = katalog.WyszukajPoNazwie(nazwaProduktuZD);
                        Console.WriteLine("Podaj nowa ilosc: ");
                        nowaIlosc = Convert.ToInt32(Console.ReadLine());

                        porduktZD[0]._dostepnosc = nowaIlosc;
                        break;
                    case 5:
                        string nazwaN = "";
                        string hasloN = "";

                        Console.WriteLine("Podaj nazwe: ");
                        nazwaN = Console.ReadLine();
                        Console.WriteLine("Podaj haslo: ");
                        hasloN = Console.ReadLine();

                        zarzadanie.Rejestracja(nazwaN, hasloN, true);
                        break;
                    case 6:
                        Console.WriteLine("Do zobaczenie");
                        zamkij = true;
                        break;
                    default:
                        Console.WriteLine("Niepoprawna opercja, srpobuj ponownie");
                        break;
                }
            }
        }
    }    
}