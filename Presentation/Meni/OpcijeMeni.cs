using System;
using Domain.Servisi;

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

        private readonly PonudaVinaMeni ponudaVinaMeni;

        public OpcijeMeni(
            IEvidencijaProizvodnjeVinaServis evidencijaVinaServis,
            PaleteMeni paleteMeni,
            PakovanjeMeni pakovanjeMeni,
            IBerbaLozeServis berbaLozeServis,
            IProracunGrozdjaServis proracunGrozdjaServis,
            IVinovaLozaServis vinovaLozaServis,
            PonudaVinaMeni ponudaVinaMeni)
        {
            this.evidencijaVinaServis = evidencijaVinaServis;
            this.paleteMeni = paleteMeni;
            this.pakovanjeMeni = pakovanjeMeni;

            this.berbaLozeServis = berbaLozeServis;
            this.proracunGrozdjaServis = proracunGrozdjaServis;
            this.vinovaLozaServis = vinovaLozaServis;
            this.ponudaVinaMeni = ponudaVinaMeni;
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
                Console.WriteLine("7) Pregled ponude vina");
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
                        ponudaVinaMeni.Prikazi();
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
