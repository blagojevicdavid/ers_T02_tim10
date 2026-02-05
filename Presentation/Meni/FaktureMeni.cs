using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Meni
{
    public class FaktureMeni
    {
        private readonly IFakturePregledServis fakturePregledServis;

        public FaktureMeni(IFakturePregledServis fakturePregledServis)
        {
            this.fakturePregledServis = fakturePregledServis;
        }

        public void Prikazi()
        {
            Console.WriteLine("\n=== Pregled izdatih računa ===");

            var fakture = fakturePregledServis.PreuzmiSveFakture();

            if (fakture.Count == 0)
            {
                Console.WriteLine("Nema izdatih faktura.");
                Pauza();
                return;
            }

            foreach (var f in fakture)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"Faktura ID: {f.Id}");
                Console.WriteLine($"Datum:      {f.DatumIzdavanja}");
                Console.WriteLine($"Tip prodaje:{f.TipProdaje}");
                Console.WriteLine($"Placanje:   {f.NacinPlacanja}");

                if (f.Stavke != null && f.Stavke.Count > 0)
                {
                    Console.WriteLine("Stavke:");
                    foreach (var s in f.Stavke)
                        Console.WriteLine($" - VinoId: {s.VinoId} | Kolicina: {s.Kolicina} | Cena: {s.CenaPoKomadu}");
                }
            }

            Pauza();
        }

        private static void Pauza()
        {
            Console.WriteLine("\nEnter za nastavak...");
            Console.ReadLine();
        }
    }
}
