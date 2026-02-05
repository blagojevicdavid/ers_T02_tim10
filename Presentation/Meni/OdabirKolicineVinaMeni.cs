using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Meni
{
    public class OdabirKolicineVinaMeni
    {
        private readonly IPonudaVinaServis ponudaVinaServis;
        private readonly IOdabirKolicineVinaServis odabirKolicineServis;

        public OdabirKolicineVinaMeni(
            IPonudaVinaServis ponudaVinaServis,
            IOdabirKolicineVinaServis odabirKolicineServis)
        {
            this.ponudaVinaServis = ponudaVinaServis;
            this.odabirKolicineServis = odabirKolicineServis;
        }

        public void Prikazi()
        {
            Console.Clear();
            Console.WriteLine(" ODABIR KOLICINE VINA ");

            var ponuda = ponudaVinaServis.VratiPonudu();
            if (ponuda.Count == 0)
            {
                Console.WriteLine("Nema vina u ponudi.");
                Console.WriteLine("Pritisni ENTER za povratak...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Dostupna vina:");
            foreach (var v in ponuda)
                Console.WriteLine($" Sifra: {v.Sifra} | {v.Naziv} | {v.Kategorija}");

            Console.WriteLine();
            Console.Write("Unesi SIFRU vina (ENTER za izlaz): ");
            var sifra = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(sifra)) return;

            Console.Write("Unesi KOLICINU (ceo broj): ");
            var kolicinaInput = Console.ReadLine();
            if (!int.TryParse(kolicinaInput, out int kolicina))
            {
                Console.WriteLine("Neispravan unos kolicine.");
                Console.WriteLine("Pritisni ENTER za povratak...");
                Console.ReadLine();
                return;
            }

            var rezultat = odabirKolicineServis.Odaberi(sifra, kolicina);
            if (rezultat == null)
            {
                Console.WriteLine("Ne postoji vino sa tom sifrom ili je kolicina neispravna.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"Izabrali ste: {rezultat.Vino.Naziv}");
            Console.WriteLine($"Kolicina: {rezultat.Kolicina}");
        }
    }
}
