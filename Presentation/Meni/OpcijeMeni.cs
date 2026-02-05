using System;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        
        private readonly PonudaVinaMeni ponudaVinaMeni;
        private readonly OdabirKolicineVinaMeni odabirKolicineVinaMeni;
        private readonly ProdajaMeni prodajaMeni;
        private readonly FaktureMeni faktureMeni;

        private readonly VinovaLozaMeni vinovaLozaMeni;
        private readonly BerbaLozeMeni berbaLozeMeni;
        private readonly ProracunGrozdjaMeni proracunGrozdjaMeni;

        
        private readonly FermentacijaMeni fermentacijaMeni;
        private readonly ProizvodnjaVinaMeni proizvodnjaVinaMeni;
        private readonly PakovanjeMeni pakovanjeMeni;
        private readonly PaleteMeni paleteMeni;
        private readonly SkladistenjeMeni skladistenjeMeni;
        private readonly IsporukaVinaMeni isporukaVinaMeni;

        public OpcijeMeni(
            PonudaVinaMeni ponudaVinaMeni,
            OdabirKolicineVinaMeni odabirKolicineVinaMeni,
            ProdajaMeni prodajaMeni,
            FaktureMeni faktureMeni,
            VinovaLozaMeni vinovaLozaMeni,
            BerbaLozeMeni berbaLozeMeni,
            ProracunGrozdjaMeni proracunGrozdjaMeni,
            FermentacijaMeni fermentacijaMeni,
            ProizvodnjaVinaMeni proizvodnjaVinaMeni,
            PakovanjeMeni pakovanjeMeni,
            PaleteMeni paleteMeni,
            SkladistenjeMeni skladistenjeMeni,
            IsporukaVinaMeni isporukaVinaMeni)
        {
            this.ponudaVinaMeni = ponudaVinaMeni;
            this.odabirKolicineVinaMeni = odabirKolicineVinaMeni;
            this.prodajaMeni = prodajaMeni;
            this.faktureMeni = faktureMeni;

            this.vinovaLozaMeni = vinovaLozaMeni;
            this.berbaLozeMeni = berbaLozeMeni;
            this.proracunGrozdjaMeni = proracunGrozdjaMeni;

            this.fermentacijaMeni = fermentacijaMeni;
            this.proizvodnjaVinaMeni = proizvodnjaVinaMeni;
            this.pakovanjeMeni = pakovanjeMeni;
            this.paleteMeni = paleteMeni;
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
                Console.WriteLine("5) Vinograd i berba");
                Console.WriteLine("0) Izlaz");
                Console.Write("\nIzbor: ");

                switch (Console.ReadLine())
                {
                    case "1":

                        ponudaVinaMeni.Prikazi();   

                     
                        break;

                    case "2":
                        odabirKolicineVinaMeni.Prikazi();
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

                    case "5":
                        PrikaziVinogradMeni();
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

        private void PrikaziVinogradMeni()
        {
            bool izlaz = false;

            while (!izlaz)
            {
                Console.Clear();
                Console.WriteLine("=== VINOGRAD I BERBA ===");
                Console.WriteLine("1) Vinova loza");
                Console.WriteLine("2) Berba loze");
                Console.WriteLine("3) Proračun grožđa");
                Console.WriteLine("0) Nazad");
                Console.Write("\nIzbor: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        vinovaLozaMeni.Prikazi();
                        Pauza();
                        break;

                    case "2":
                        berbaLozeMeni.Prikazi();
                        Pauza();
                        break;

                    case "3":
                        proracunGrozdjaMeni.Prikazi();
                        Pauza();
                        break;

                    case "0":
                        izlaz = true;
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
                Console.WriteLine("Prijavljeni ste kao -> Kelar majstor");
                Console.WriteLine("===================================");
                Console.WriteLine("1) Fermentacija");
                Console.WriteLine("2) Proizvodnja vina");
                Console.WriteLine("3) Pakovanje");
                Console.WriteLine("4) Palete");
                Console.WriteLine("5) Skladištenje");
                Console.WriteLine("6) Isporuka vina");
                Console.WriteLine("0) Izlaz");
                Console.Write("\nIzbor: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        fermentacijaMeni.Prikazi();
                        Pauza();
                        break;

                    case "2":
                        proizvodnjaVinaMeni.Prikazi();
                        Pauza();
                        break;

                    case "3":
                        pakovanjeMeni.Prikazi();
                        Pauza();
                        break;

                    case "4":
                        paleteMeni.Prikazi();
                        Pauza();
                        break;

                    case "5":
                        skladistenjeMeni.Prikazi();
                        Pauza();
                        break;

                    case "6":
                        isporukaVinaMeni.Prikazi();
                        Pauza();

                        break;

                    case "0":
                        izlaz = true;
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
