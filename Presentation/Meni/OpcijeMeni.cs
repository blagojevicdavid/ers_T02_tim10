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

        // ovde navesti ostale servise
        public OpcijeMeni(
            IFermentacijaServis fermentacijaServis,
            IMerenjeSeceraServis merenjeSeceraServis,
            IEvidencijaProizvodnjeVinaServis evidencijaVinaServis,
            PaleteMeni paleteMeni
            /*ovde navesti ostale servise*/)
        {
            this.fermentacijaServis = fermentacijaServis;
            this.merenjeSeceraServis = merenjeSeceraServis;
            this.evidencijaVinaServis = evidencijaVinaServis;

            // ✅ DODATO
            this.paleteMeni = paleteMeni;

            // i ovde ispuniti za ostale
        }

        public void PrikaziMeni()
        {
            Console.WriteLine("\n============================================ Meni ===========================================");
            Console.WriteLine("Odaberite jednu od sledećih opcija:");
            Console.WriteLine("1) Meni fermentacije");
            Console.WriteLine("2) Proizvodnja vina (gotovi proizvodi)");
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
                        // ovde dodati ostale menije
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
