using System;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        
        private readonly PonudaVinaMeni ponudaVinaMeni;
        private readonly OdabirKolicineVinaMeni odabirKolicineVinaMeni;
        private readonly ProdajaMeni prodajaMeni;
        private readonly FaktureMeni faktureMeni;
        private readonly ProizvodnjaVinaMeni proizvodnjaVinaMeni;
        private readonly PakovanjeMeni pakovanjeMeni;
        private readonly SkladistenjeMeni skladistenjeMeni;
        private readonly IsporukaVinaMeni isporukaVinaMeni;

        public OpcijeMeni(PonudaVinaMeni ponudaVinaMeni,OdabirKolicineVinaMeni odabirKolicineVinaMeni,ProdajaMeni prodajaMeni,FaktureMeni faktureMeni,
            ProizvodnjaVinaMeni proizvodnjaVinaMeni,PakovanjeMeni pakovanjeMeni,SkladistenjeMeni skladistenjeMeni,IsporukaVinaMeni isporukaVinaMeni)
        {
            this.ponudaVinaMeni = ponudaVinaMeni;
            this.odabirKolicineVinaMeni = odabirKolicineVinaMeni;
            this.prodajaMeni = prodajaMeni;
            this.faktureMeni = faktureMeni;
            this.proizvodnjaVinaMeni = proizvodnjaVinaMeni;
            this.pakovanjeMeni = pakovanjeMeni;
            this.skladistenjeMeni = skladistenjeMeni;
            this.isporukaVinaMeni = isporukaVinaMeni;
        }
        public void PrikaziEnolog()
        {
            bool izlaz = false;

            while (!izlaz)
            {
                Console.Clear();
                Console.WriteLine("Prijavljeni ste kao -> Glavni enolog");
                Console.WriteLine("===================================");
                Console.WriteLine("1) Ponuda vina");
                Console.WriteLine("2) Vina na stanju");
                Console.WriteLine("3) Prodaja vina");
                Console.WriteLine("4) Pregled faktura");
                Console.WriteLine("0) Izlaz");
                Console.Write("\nIzbor: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        odabirKolicineVinaMeni.Prikazi();
                        Pauza();
                        break;

                    case "2":
                        ponudaVinaMeni.Prikazi();
                        Pauza();
                        break;

                    case "3":
                        prodajaMeni.Prikazi();
                        Pauza();
                        break;

                    case "4":
                        faktureMeni.Prikazi();
                        Pauza();
                        break;
                      
                    case "0":
                        izlaz = true;
                        break;

                    default:
                        Pauza();
                        break;
                }
            }
        }

        public void PrikaziKelarMajstor()
        {
            bool izlaz = false;

            while (!izlaz)
            {
                Console.Clear();
                Console.WriteLine("Prijavljeni ste kao -> Glavni enolog");
                Console.WriteLine("===================================");
                Console.WriteLine("1) Ponuda vina");
                Console.WriteLine("2) Vina na stanju");
                Console.WriteLine("3) Prodaja vina");
                Console.WriteLine("0) Izlaz");
                Console.Write("\nIzbor: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        odabirKolicineVinaMeni.Prikazi();
                        Pauza();
                        break;

                    case "2":
                        ponudaVinaMeni.Prikazi();
                        Pauza();
                        break;

                    case "3":
                        prodajaMeni.Prikazi();
                        Pauza();
                        break;

                    case "0":
                        izlaz = true;
                        break;

                    default:
                        Pauza();
                        break;
                }
            }
        }
        private static void Pauza()
        {
            Console.WriteLine("\nPritisni ENTER za nastavak...");
            Console.ReadLine();
        }
    }
}
