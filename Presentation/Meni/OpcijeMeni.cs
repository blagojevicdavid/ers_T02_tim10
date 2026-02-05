using System;
using Domain.Servisi;
using Services.IsporukaServis;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        private readonly IEvidencijaProizvodnjeVinaServis evidencijaVinaServis;
        private readonly PaleteMeni paleteMeni;
        private readonly PakovanjeMeni pakovanjeMeni;

        private readonly IBerbaLozeServis berbaLozeServis;
        private readonly IProracunGrozdjaServis proracunGrozdjaServis;
        private readonly IVinovaLozaServis vinovaLozaServis;

        private readonly IIsporukaVinaServis isporukaVinaServis;

        private readonly PonudaVinaMeni ponudaVinaMeni;
        private readonly OdabirKolicineVinaMeni odabirKolicineVinaMeni;
        private readonly ProdajaMeni prodajaMeni;
        private readonly FaktureMeni faktureMeni;


        public OpcijeMeni(
            IEvidencijaProizvodnjeVinaServis evidencijaVinaServis,
            PaleteMeni paleteMeni,
            PakovanjeMeni pakovanjeMeni,
            IBerbaLozeServis berbaLozeServis,
            IProracunGrozdjaServis proracunGrozdjaServis,
            IVinovaLozaServis vinovaLozaServis,
            IIsporukaVinaServis isporukaVinaServis,
            PonudaVinaMeni ponudaVinaMeni,
            OdabirKolicineVinaMeni odabirKolicineVinaMeni,
            ProdajaMeni prodajaMeni,
            FaktureMeni faktureMeni)
        {
            this.evidencijaVinaServis = evidencijaVinaServis;
            this.paleteMeni = paleteMeni;
            this.pakovanjeMeni = pakovanjeMeni;

            this.berbaLozeServis = berbaLozeServis;
            this.proracunGrozdjaServis = proracunGrozdjaServis;
            this.vinovaLozaServis = vinovaLozaServis;

            this.isporukaVinaServis = isporukaVinaServis;

            this.ponudaVinaMeni = ponudaVinaMeni;
            this.odabirKolicineVinaMeni = odabirKolicineVinaMeni;
            this.prodajaMeni = prodajaMeni;
            this.faktureMeni = faktureMeni;
        }

        public void PrikaziEnolog()
        {
            bool izlaz = false;

            while (!izlaz)
            {
                Console.Clear();
                Console.WriteLine($"Prijavljeni ste kao -> Glavni enolog");
                Console.WriteLine("============================================");
                Console.WriteLine("                 GLAVNI MENI               ");
                Console.WriteLine("============================================");
                Console.WriteLine("1) Ponuda vina");
                Console.WriteLine("2) Vina na stanju");
                Console.WriteLine("3) Prodaja vina");
                Console.WriteLine("4) Pregled faktura"); // ovo ostaje
                Console.WriteLine("5) Ostale opcije");


                //Console.WriteLine("3) Pakovanje / slanje u skladiste");
                //Console.WriteLine("4) Berba loze");
                
                //Console.WriteLine("7) Isporuka vina (zahtjev servisu prodaje)");
                //Console.WriteLine("8) Pregled ponude vina");
                //Console.WriteLine("9) Odabir kolicine vina");
                //Console.WriteLine("10) Isporuka vina kupcu");


                Console.WriteLine("0) Izlaz");
                Console.Write("\nIzbor: ");

                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        //dodati prikaz ponude vina, dodati poziv na prikaz ponude vina, lista vina u ponudi
                        Pauza();
                        break;

                    case "2":
                        // isto kao 1, samo vina koja su na stanju
                        Pauza();
                        break;

                    case "3":
                        //vodi ka prodaji vina
                        Pauza();
                        break;

                    case "4":
                        faktureMeni.Prikazi();
                        Pauza();
                        break;

                    case "5":
                        //dodati ostale opcije kao poseban meni
                        Pauza();
                        break;

                        /*
                    case "6":
                        new VinovaLozaMeni(vinovaLozaServis).Prikazi();
                        Pauza();
                        break;

                    case "7":
                        new IsporukaVinaMeni(isporukaVinaServis).Prikazi();
                        Pauza();
                        break;

                    case "8":
                        ponudaVinaMeni.Prikazi();
                        Pauza();
                        break;

                    case "9":
                        odabirKolicineVinaMeni.Prikazi();
                        Pauza();
                        break;

                    case "10":
                        prodajaMeni.Prikazi();
                        break;

                    case "11":
                        faktureMeni.Prikazi();
                        break;
                        */

                    case "0":
                        izlaz = true;
                        break;

                    default:
                        Console.WriteLine("\nNepoznata opcija. Pokušaj ponovo.");
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
                Console.WriteLine($"Prijavljeni ste kao -> Kelar majstor");
                Console.WriteLine("============================================");
                Console.WriteLine("                 GLAVNI MENI               ");
                Console.WriteLine("============================================");
                Console.WriteLine("1) "); //prva opcija kelar majstora
                Console.WriteLine();



                Console.WriteLine("0) Izlaz");
                Console.Write("\nIzbor: ");

                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        //prva opcija kelar majstora
                        Pauza();
                        break;


                    case "0":
                        izlaz = true;
                        break;

                    default:
                        Console.WriteLine("\nNepoznata opcija. Pokušaj ponovo.");
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
