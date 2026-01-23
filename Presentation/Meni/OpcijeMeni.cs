using System;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        private readonly IFermentacijaServis fermentacijaServis;
        private readonly IMerenjeSeceraServis merenjeSeceraServis;
        private readonly IEvidencijaProizvodnjeVinaServis evidencijaVinaServis;
        

        // ✅ DODATO
        private readonly PaleteMeni paleteMeni;
        private readonly IBerbaLozeServis berbaLozeServis;
        private readonly IProracunGrozdjaServis proracunGrozdjaServis;
        private readonly IVinovaLozaServis vinovaLozaServis;

        // ovde navesti ostale servise
        public OpcijeMeni(
            IFermentacijaServis fermentacijaServis,
            IMerenjeSeceraServis merenjeSeceraServis,
            IEvidencijaProizvodnjeVinaServis evidencijaVinaServis,
            PaleteMeni paleteMeni,
            IBerbaLozeServis berbaLozeServis,
            IProracunGrozdjaServis proracunGrozdjaServis,
            IVinovaLozaServis vinovaLozaServis
            /*ovde navesti ostale servise*/)
        {
            this.fermentacijaServis = fermentacijaServis;
            this.merenjeSeceraServis = merenjeSeceraServis;
            this.evidencijaVinaServis = evidencijaVinaServis;

            // ✅ DODATO
            this.paleteMeni = paleteMeni;

            // i ovde ispuniti za ostale
            this.berbaLozeServis = berbaLozeServis;
            this.proracunGrozdjaServis = proracunGrozdjaServis;
            this.vinovaLozaServis = vinovaLozaServis;

        }

        public void PrikaziMeni()
        {
            Console.WriteLine("\n============================================ Meni ===========================================");
            Console.WriteLine("Odaberite jednu od sledećih opcija:");
            Console.WriteLine("1) Meni fermentacije");
            Console.WriteLine("2) Proizvodnja vina (gotovi proizvodi)");
            Console.WriteLine("3) Berba loze");
            Console.WriteLine("4) Proračun grožđa");
            Console.WriteLine("5) Sadnja vinove loze");
            Console.WriteLine("0) Izlaz");

            bool kraj = false;
            while (!kraj)
            {
                Console.Write("\nIzbor: ");
                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        new FermentacijaMeni(fermentacijaServis, merenjeSeceraServis /*ovde ostatak servis*/).Prikazi();
                        Console.WriteLine("\n============================================ Meni ===========================================");
                        Console.WriteLine("Odaberite jednu od sledećih opcija:");
                        Console.WriteLine("1) Meni fermentacije");
                        Console.WriteLine("2) Proizvodnja vina (gotovi proizvodi)");
                        Console.WriteLine("3) Berba loze");
                        Console.WriteLine("4) Proračun grožđa");
                        Console.WriteLine("5) Sadnja vinove loze");
                        // ovde dodati ostale menije


                        Console.WriteLine("0) Izlaz");
                        break;

                    case "0":
                        kraj = true;
                        break;

                    case "2":
                        // ✅ MINIMALNO: sada prosleđujemo paleteMeni u “gotov proizvod”
                        new ProizvodnjaVinaMeni(evidencijaVinaServis, paleteMeni).Prikazi();

                        Console.WriteLine("\n============================================ Meni ===========================================");
                        Console.WriteLine("Odaberite jednu od sledećih opcija:");
                        Console.WriteLine("1) Meni fermentacije");
                        Console.WriteLine("2) Proizvodnja vina (gotovi proizvodi)");
                        Console.WriteLine("3) Berba loze");
                        Console.WriteLine("4) Proračun grožđa");
                        Console.WriteLine("5) Sadnja vinove loze");
                        // ovde dodati ostale menije
                        Console.WriteLine("0) Izlaz");
                        break;

                    case "3":
                        new BerbaLozeMeni(berbaLozeServis).Prikazi();

                        Console.WriteLine("\n============================================ Meni ===========================================");
                        Console.WriteLine("Odaberite jednu od sledećih opcija:");
                        Console.WriteLine("1) Meni fermentacije");
                        Console.WriteLine("2) Proizvodnja vina (gotovi proizvodi)");
                        Console.WriteLine("3) Berba loze");
                        Console.WriteLine("4) Proračun grožđa");
                        Console.WriteLine("5) Sadnja vinove loze");
                        Console.WriteLine("0) Izlaz");
                        break;

                    case "4":
                        new ProracunGrozdjaMeni(proracunGrozdjaServis).Prikazi();

                        Console.WriteLine("\n============================================ Meni ===========================================");
                        Console.WriteLine("Odaberite jednu od sledećih opcija:");
                        Console.WriteLine("1) Meni fermentacije");
                        Console.WriteLine("2) Proizvodnja vina (gotovi proizvodi)");
                        Console.WriteLine("3) Berba loze");
                        Console.WriteLine("4) Proračun grožđa");
                        Console.WriteLine("5) Sadnja vinove loze");
                        Console.WriteLine("0) Izlaz");
                        break;

                    case "5":
                        new VinovaLozaMeni(vinovaLozaServis).Prikazi();
                        Console.WriteLine("\n============================================ Meni ===========================================");
                        Console.WriteLine("Odaberite jednu od sledećih opcija:");
                        Console.WriteLine("1) Meni fermentacije");
                        Console.WriteLine("2) Proizvodnja vina (gotovi proizvodi)");
                        Console.WriteLine("3) Berba loze");
                        Console.WriteLine("4) Proračun grožđa");
                        Console.WriteLine("5) Sadnja vinove loze");
                        Console.WriteLine("0) Izlaz");

                        break;

                    default:
                        Console.WriteLine("Nepoznata opcija. Pokusaj ponovo.");
                        break;
                }
            }
        }
    }
}
