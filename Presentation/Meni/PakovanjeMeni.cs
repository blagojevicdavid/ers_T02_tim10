using System;
using Domain.Enumeracije;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class PakovanjeMeni
    {
        private readonly IPakovanjeServis pakovanjeServis;

        public PakovanjeMeni(IPakovanjeServis pakovanjeServis)
        {
            this.pakovanjeServis = pakovanjeServis;
        }

        public void Prikazi()
        {
            Console.WriteLine("\n--- PRIPREMA VINA ZA PRODAJU (Pakovanje + Slanje u skladiste) ---");

            Console.Write("Naziv vina: ");
            var naziv = Console.ReadLine();

            if (naziv == null || naziv.Trim() == "")
            {
                Console.WriteLine("Naziv ne sme biti prazan.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Kategorija (izaberi broj): 0-Stolno, 1-Kvalitetno, 2-Premium");
            Console.Write("Kategorija: ");
            if (!int.TryParse(Console.ReadLine(), out int katInt) || katInt < 0 || katInt > 2)
            {
                Console.WriteLine("Neispravna kategorija.");
                Console.ReadLine();
                return;
            }
            KategorijaVina kategorija = (KategorijaVina)katInt;

            Console.Write("Broj flasa: ");
            if (!int.TryParse(Console.ReadLine(), out int brojFlasa) || brojFlasa <= 0)
            {
                Console.WriteLine("Broj flasa mora biti > 0.");
                Console.ReadLine();
                return;
            }

            Console.Write("Zapremina flase (npr 0.75): ");
            if (!double.TryParse(Console.ReadLine(), out double zapremina) || zapremina <= 0)
            {
                Console.WriteLine("Zapremina mora biti > 0.");
                Console.ReadLine();
                return;
            }

            Console.Write("Adresa odredista: ");
            var adresa = Console.ReadLine();

            if (adresa == null || adresa.Trim() == "")
            {
                Console.WriteLine("Adresa ne sme biti prazna.");
                Console.ReadLine();
                return;
            }

            Console.Write("Vinski podrum ID (GUID): ");
            var guidInput = Console.ReadLine();

            if (guidInput == null || guidInput.Trim() == "")
            {
                Console.WriteLine("ID ne sme biti prazan.");
                Console.ReadLine();
                return;
            }

            guidInput = guidInput.Trim();

            if (!Guid.TryParse(guidInput, out Guid podrumId))
            {
                Console.WriteLine("Neispravan GUID format.");
                Console.WriteLine("Primjer: aa111111-bbbb-4444-8888-000000000001");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Unesen podrum ID: {podrumId}");
            Console.WriteLine("Pritisni ENTER da pokreneš slanje...");
            Console.ReadLine();

            var (ok, paleta) = pakovanjeServis.PosaljiPrvuDostupnuUpakovanuPaletu(
                naziv, kategorija, brojFlasa, zapremina, adresa, podrumId
            );

            if (!ok)
            {
                Console.WriteLine("Neuspesno!");
                Console.WriteLine("- Provjeri da li postoji vino sa tim nazivom + kategorijom + zapreminom.");
                Console.WriteLine("- Provjeri da li unosiš tačan GUID podruma iz XML-a.");
                Console.WriteLine("- Provjeri da li skladistenje prihvata paletu (kapacitet/pravila).");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"USPESNO! Paleta: {paleta.Sifra} | Status: {paleta.Status} | PodrumId: {paleta.VinskiPodrumId}");
            Console.WriteLine("Pritisni ENTER za nastavak...");
            Console.ReadLine();
        }
    }
}
