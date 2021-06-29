using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KontoBankowe
{
    class Program
    {
        private static readonly string KatalogKont = "konta";
        private static readonly string PlikZKontami = "uzytkownicy.txt";
        private static List<Klient> ListaKlientow;
        private static List<Konto> ListaKont;
        private static Konto AktywneKonto;

        static void Main(string[] args)
        {
            ZaladowanieProgramu();
            StartProgramu();
            ZamkniecieProgramu();

            Console.WriteLine("\nWylogowano z aplikacji.");
            Console.ReadKey();
        }

        private static void ZaladowanieProgramu()
        {
            ListaKlientow = new List<Klient>();
            ListaKont = new List<Konto>();

            if (File.Exists(PlikZKontami))
            {
                string[] klienci = File.ReadAllLines(PlikZKontami);
                foreach (string klient in klienci)
                {
                    string[] parsowanie = klient.Split(' ');
                    Klient pojedynczyKlient = new Klient(parsowanie[0], parsowanie[1], parsowanie[2]);
                    ListaKlientow.Add(pojedynczyKlient);
                }
            }

            if (Directory.Exists(KatalogKont))
            {
                string[] plikiKont = Directory.GetFiles(KatalogKont);
                foreach (string plik in plikiKont)
                {
                    string daneKonta = File.ReadAllText(plik);
                    string[] parsowanie = daneKonta.Split(' ');
                    Klient wlasciciel = ListaKlientow.FirstOrDefault(x => x.Imie == parsowanie[0] && x.Nazwisko == parsowanie[1]);
                    Konto pojedynczeKonto = new Konto(wlasciciel, Double.Parse(parsowanie[2]));
                    ListaKont.Add(pojedynczeKonto);
                }
            }
        }

        private static void StartProgramu()
        {
            Console.Clear();
            Console.WriteLine("Program odwzorowuje Konto Bankowe. Użytkownik ma możliwość dokonania:.");

            int countDown = 0;
            int[] opcje = { 1, 2, 3 };

            Menu();

            do
            {
                try
                {
                    int operacja = int.Parse(Console.ReadLine());

                    if(opcje.Any(x => x == operacja))
                    {
                        switch (operacja)
                        {
                            case 1:
                                Logowanie();
                                break;
                            case 2:
                                Rejestracja();
                                break;
                            case 3:
                                countDown = 3;
                                Console.WriteLine("Wylogowuje...");
                                ZamkniecieProgramu();
                                break;
                            default:
                                break;

                        }
                    }           
                }
                catch
                {
                    Console.Clear();
                    countDown++;
                    Console.WriteLine($"Nieprawidlowe dane wejściowe. Liczba nieprawidlowych prob: {countDown}.\nProsze spróbuj ponownie...\n");
                    Menu();
                }
                
            }
            while (countDown < 3);
        }

        private static void Logowanie()
        {
            Console.Write("Podaj imię: ");
            string imie = Console.ReadLine();
            Console.Write("\nPodaj nazwisko: ");
            string nazwisko = Console.ReadLine();
            Console.Write("\nPodaj hasło: ");
            string haslo = Console.ReadLine();

            Console.Clear();

            Konto konto = ListaKont.FirstOrDefault(x => x.Klient.Imie == imie && x.Klient.Nazwisko == nazwisko && x.Klient.Haslo == haslo);
            if (konto != null)
            {
                AktywneKonto = ListaKont.FirstOrDefault(x => x.Klient.Imie == imie && x.Klient.Nazwisko == nazwisko && x.Klient.Haslo == haslo);
                OperacjeNaKoncie();
            }
            else
            {
                Console.Write("\nKonto nie istnieje. Czy chcesz stworzyć nowe konto? [T/N]: ");
                string stworzycKonto = Console.ReadLine();
                if (stworzycKonto.ToLower() == "t")
                {
                    Rejestracja();
                }
                else
                {
                    StartProgramu();
                }
            }
            
        }

        private static void OperacjeNaKoncie()
        {
            Console.WriteLine("1. Wpłata\n2. Wyplata\n3. Stan Konta\n4. Wylogowanie");

            int[] opcje = { 1, 2, 3 };
            int countDown = 0;

            do
            {
                try
                {
                    int operacja = int.Parse(Console.ReadLine());
                    countDown = 1;

                    switch (operacja)
                    {
                        case 1:
                            Console.Clear();
                            Console.Write("Podaj kwotę wpłaty: ");
                            string wplata = Console.ReadLine();
                            AktywneKonto.Wplata(Double.Parse(wplata));
                            AktywneKonto.PokazStanKonta();
                            OperacjeNaKoncie();
                            break;
                        case 2:
                            Console.Clear();
                            Console.Write("Podaj kwotę wypłaty: ");
                            string wyplata = Console.ReadLine();
                            AktywneKonto.Wyplata(Double.Parse(wyplata));
                            AktywneKonto.PokazStanKonta();
                            OperacjeNaKoncie();
                            break;
                        case 3:
                            Console.Clear();
                            AktywneKonto.PokazStanKonta();
                            OperacjeNaKoncie();
                            break;
                        case 4:
                            Console.Clear();
                            countDown = 1;
                            StartProgramu();
                            break;
                    }
                }
                catch
                {
                    Console.WriteLine("Nie prawidlowa opcja. Sprobuj ponownie...");
                }
            }
            while (countDown!=1);
        }

        private static void Rejestracja()
        {
            Console.WriteLine("Podaj imię: ");
            string imie = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko: ");
            string nazwisko = Console.ReadLine();
            string haslo;
            bool walidacja;

            do
            {
                Console.WriteLine("Podaj hasło: ");
                haslo = Console.ReadLine();
                walidacja = WalidacjaHasla(haslo);

                if(!walidacja)
                {
                    
                    Console.WriteLine("Haslo musi zawierac conajmniej 8 znakow, jedna duza litere oraz cyfre.");
                }
            }
            while(!walidacja);
            

            Klient nowyKlient = new Klient(imie, nazwisko, haslo);
            Konto noweKonto = new Konto(nowyKlient, 0.0);

            ListaKlientow.Add(nowyKlient);
            ListaKont.Add(noweKonto);
            AktywneKonto = noweKonto;

            OperacjeNaKoncie();
            
        }

        private static void ZamkniecieProgramu()
        {

            StringBuilder obslugaPliku = new StringBuilder();
            foreach(Klient klient in ListaKlientow)
            {
                obslugaPliku.Append(klient.Imie + " " + klient.Nazwisko + " " + klient.Haslo + "\n");
            }
            File.WriteAllText(PlikZKontami, obslugaPliku.ToString());

            if(Directory.Exists(KatalogKont))
            {
                Directory.Delete(KatalogKont, true);
            }
            Directory.CreateDirectory(KatalogKont);

            foreach(Konto konto in ListaKont)
            {
                string plik = $"{konto.Klient.Imie}_{konto.Klient.Nazwisko}.txt";
                string zawartosc = konto.Klient.Imie + " " + konto.Klient.Nazwisko + " " + konto.StanKonta.ToString();
                File.WriteAllText(Path.Combine(KatalogKont, plik), zawartosc);
            }
            System.Environment.Exit(0);
        }

        private static void Menu()
        {
            
            Console.WriteLine("1. Logowanie");
            Console.WriteLine("2. Rejestracja konta");
            Console.WriteLine("3. Zamknięcie aplikacji");
            Console.Write("Podaj jaką operację chcesz wykonać [1-3]:");
        }

        private static bool WalidacjaHasla(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            var isValidated = hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password);

            return isValidated;
        }

    }
}
