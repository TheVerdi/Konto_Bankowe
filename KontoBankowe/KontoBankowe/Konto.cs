namespace KontoBankowe
{
    internal class Konto
    {
        private Klient _klient;
        private double _stanKonta;

        public Klient Klient { get => _klient; set => _klient = value; }
        public double StanKonta { get => _stanKonta; set => _stanKonta = value; }
        public Konto(Klient klient, double stanKonta)
        {
            this.Klient = klient;
            this.StanKonta = stanKonta;
        }

        public void Wplata(double value)
        {
            this.StanKonta += value;
        }

        public void Wyplata(double value)
        {
            this.StanKonta -= value;
        }

        public void PokazStanKonta()
        {
            System.Console.WriteLine("Stan konta wynosi: " + this.StanKonta);
        }
    }
}