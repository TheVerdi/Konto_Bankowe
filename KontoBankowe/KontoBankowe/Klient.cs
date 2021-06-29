namespace KontoBankowe
{
    public class Klient
    {
        private string _imie;
        private string _nazwisko;
        private string _haslo;

        public string Imie { get => _imie; set => _imie = value; }
        public string Nazwisko { get => _nazwisko; set => _nazwisko = value; }
        public string Haslo { get => _haslo; set => _haslo = value; }

        public Klient(string imie, string nazwisko, string haslo)
        {
            this.Imie = imie;
            this.Nazwisko = nazwisko;
            this.Haslo = haslo;
        }
    }
}