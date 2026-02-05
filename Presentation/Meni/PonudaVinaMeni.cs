using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Meni
{
    public class PonudaVinaMeni
    {
        private readonly IPonudaVinaServis ponudaServis;

        public PonudaVinaMeni(IPonudaVinaServis ponudaServis)
        {
            this.ponudaServis = ponudaServis;
        }

        public void Prikazi()
        {
            Console.Clear();
            Console.WriteLine("=== PREGLED PONUDE VINA ===");

            var ponuda = ponudaServis.VratiPonudu();
            if (ponuda.Count == 0)
            {
                Console.WriteLine("Nema vina u ponudi.");
                Console.WriteLine("Pritisni ENTER za nastavak");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Dostupna vina:");
            foreach (var v in ponuda)
            {
                Console.WriteLine($"- Sifra: {v.Sifra} | {v.Naziv} | {v.Kategorija}");
            }

            Console.WriteLine();
            Console.Write("Unesi SIFRU vina koje želiš za prodaju (ili ENTER za izlaz): ");
            var sifra = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(sifra))
                return;
            sifra = sifra.Trim();

            var izabrano = ponudaServis.PronadjiPoSifri(sifra);
            if (izabrano == null)
            {
                Console.WriteLine("Ne postoji vino sa tom šifrom.");
                Console.WriteLine("Pritisni ENTER za nastavak");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Izabrali ste: {izabrano.Naziv} (Sifra: {izabrano.Sifra})");
        }
    }
}
