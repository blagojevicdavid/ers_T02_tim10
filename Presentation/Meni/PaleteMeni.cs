using System;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class PaleteMeni
    {
        private readonly IPaleteServis paleteServis;

        public PaleteMeni(IPaleteServis paleteServis)
        {
            this.paleteServis = paleteServis;
        }

        public void Prikazi()
        {
            Console.WriteLine("\n--- SLANJE PALETA U PODRUM---");

            Console.Write("Unesi ID vinskog podruma (GUID): ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid podrumId))
            {
                Console.WriteLine("Neispravan GUID.");
                return;
            }

            Console.Write("Unesi broj paleta (1-5): ");
            if (!int.TryParse(Console.ReadLine(), out int broj) || broj < 1 || broj > 5)
            {
                Console.WriteLine("Broj paleta mora biti od 1 do 5.");
                return;
            }

            try
            {
                var poslate = paleteServis.PosaljiPaleteUVinskiPodrum(podrumId, broj);

                Console.WriteLine($"Uspesno poslato: {poslate.Count} paleta.");
                foreach (var p in poslate)
                {
                    Console.WriteLine($"- {p.Sifra} | Status: {p.Status} | PodrumId: {p.VinskiPodrumId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greska: {ex.Message}");
            }

            Console.WriteLine("Pritisni ENTER za nastavak...");
            Console.ReadLine();
        }
    }
}
