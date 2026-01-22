using System;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        private readonly IFermentacijaServis fermentacijaServis;

        public OpcijeMeni(IFermentacijaServis fermentacijaServis /*ovde navesti ostale servise*/)
        {
            this.fermentacijaServis = fermentacijaServis;
            // i ovde ispuniti za ostale
        }

        public void PrikaziMeni()
        {
            Console.WriteLine("\n============================================ Meni ===========================================");
            Console.WriteLine("Odaberite jednu od sledećih opcija:");
            Console.WriteLine("1) Meni fermentacije");
            Console.WriteLine("0) Izlaz");

            bool kraj = false;
            while (!kraj)
            {
                Console.Write("\nIzbor: ");
                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        new FermentacijaMeni(fermentacijaServis).Prikazi();
                        Console.WriteLine("\n============================================ Meni ===========================================");
                        Console.WriteLine("Odaberite jednu od sledećih opcija:");
                        Console.WriteLine("1) Meni fermentacije");
                        Console.WriteLine("0) Izlaz");
                        break;

                    case "0":
                        kraj = true;
                        break;

                    default:
                        Console.WriteLine("Nepoznata opcija. Pokusaj ponovo.");
                        break;
                }
            }
        }
    }
}
