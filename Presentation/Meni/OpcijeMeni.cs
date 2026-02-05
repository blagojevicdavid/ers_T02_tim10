using Domain.Servisi;
using Services.IsporukaServis;
using System;

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
<<<<<<< HEAD
            IIsporukaVinaServis isporukaVinaServis)
=======
            PonudaVinaMeni ponudaVinaMeni,
            OdabirKolicineVinaMeni odabirKolicineVinaMeni,
            ProdajaMeni prodajaMeni,
            FaktureMeni faktureMeni)
>>>>>>> 82166f8ee4ed67dfcd172bde87c58d3f8fe71ed5
        {
            this.evidencijaVinaServis = evidencijaVinaServis;
            this.paleteMeni = paleteMeni;
            this.pakovanjeMeni = pakovanjeMeni;

            this.berbaLozeServis = berbaLozeServis;
            this.proracunGrozdjaServis = proracunGrozdjaServis;
            this.vinovaLozaServis = vinovaLozaServis;
<<<<<<< HEAD
            this.isporukaVinaServis = isporukaVinaServis;
=======
            this.ponudaVinaMeni = ponudaVinaMeni;
            this.odabirKolicineVinaMeni = odabirKolicineVinaMeni;
            this.prodajaMeni = prodajaMeni;
            this.faktureMeni = faktureMeni;
>>>>>>> 82166f8ee4ed67dfcd172bde87c58d3f8fe71ed5
        }

        public void Prikazi()
        {
            bool izlaz = false;

            while (!izlaz)
            {
                Console.Clear();
                Console.WriteLine("============================================");
                Console.WriteLine("                 GLAVNI MENI               ");
                Console.WriteLine("============================================");
                Console.WriteLine("1) Proizvodnja vina (gotovi proizvodi)");
                Console.WriteLine("2) Palete");
                Console.WriteLine("3) Pakovanje / slanje u skladiste");
                Console.WriteLine("4) Berba loze");
                Console.WriteLine("5) Proracun grozdja");
                Console.WriteLine("6) Vinova loza (sadnja / pregled)");
<<<<<<< HEAD
                Console.WriteLine("7) Isporuka vina (zahtjev servisu prodaje)");
=======
                Console.WriteLine("7) Pregled ponude vina");
                Console.WriteLine("8) Odabir kolicine vina");
                Console.WriteLine("9) Isporuka vina kupcu");
                Console.WriteLine("10) Pregled faktura");
>>>>>>> 82166f8ee4ed67dfcd172bde87c58d3f8fe71ed5
                Console.WriteLine("0) Izlaz");
                Console.Write("\nIzbor: ");

                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        new ProizvodnjaVinaMeni(
                            evidencijaVinaServis,
                            paleteMeni,
                            pakovanjeMeni
                        ).Prikazi();
                        Pauza();
                        break;

                    case "2":
                        paleteMeni.Prikazi();
                        Pauza();
                        break;

                    case "3":
                        pakovanjeMeni.Prikazi();
                        Pauza();
                        break;

                    case "4":
                        new BerbaLozeMeni(berbaLozeServis).Prikazi();
                        Pauza();
                        break;

                    case "5":
                        new ProracunGrozdjaMeni(proracunGrozdjaServis).Prikazi();
                        Pauza();
                        break;

                    case "6":
                        new VinovaLozaMeni(vinovaLozaServis).Prikazi();
                        Pauza();
                        break;
                    case "7":
                        new IsporukaVinaMeni(isporukaVinaServis).Prikazi();
                        Pauza();
                        break;


                    case "7":
                        ponudaVinaMeni.Prikazi();
                        Pauza();
                        break;

                    case "8":
                        odabirKolicineVinaMeni.Prikazi();
                        Pauza();
                        break;

                    case "9":
                        prodajaMeni.Prikazi();
                        break;

                    case "10":
                        faktureMeni.Prikazi();
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
