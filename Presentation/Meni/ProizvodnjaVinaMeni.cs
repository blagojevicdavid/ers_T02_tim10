using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Servisi;

namespace Presentation.Meni
{
    public class ProizvodnjaVinaMeni
    {
        private readonly IEvidencijaProizvodnjeVinaServis evidencijaVinaServis;

        public ProizvodnjaVinaMeni(IEvidencijaProizvodnjeVinaServis evidencijaVinaServis)
        {
            this.evidencijaVinaServis = evidencijaVinaServis;
        }

        public void Prikazi()
        {
            bool nazad = false;

            while (!nazad)
            {
                Console.WriteLine("\n--- PROIZVODNJA VINA (Gotovi proizvodi) ---");
                Console.WriteLine("1) Zabeleži proizvedeno vino");
                Console.WriteLine("2) Pregled svih gotovih proizvoda");
                Console.WriteLine("3) Pregled gotovih proizvoda po fermentaciji");
                Console.WriteLine("0) Nazad");
                Console.Write("Izbor: ");

                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        ZabeleziProizvedenoVino();
                        break;

                    case "2":
                        PregledSvih();
                        break;

                    case "3":
                        PregledPoFermentaciji();
                        break;

                    case "0":
                        nazad = true;
                        break;

                    default:
                        Console.WriteLine("Nepoznata opcija.");
                        break;
                }
            }
        }

        private void ZabeleziProizvedenoVino()
        {
            Console.Write("Unesi FermentacijaId (GUID): ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid fid))
            {
                Console.WriteLine("Neispravan GUID.");
                return;
            }

            Console.Write("Naziv vina: ");
            string? naziv = Console.ReadLine();

            Console.Write("Broj flaša: ");
            if (!int.TryParse(Console.ReadLine(), out int brojFlasa))
            {
                Console.WriteLine("Neispravan broj.");
                return;
            }

            Console.Write("Zapremina flaše (L, npr. 0.75 ili 1.5): ");
            if (!double.TryParse(Console.ReadLine(), out double zapremina))
            {
                Console.WriteLine("Neispravna zapremina.");
                return;
            }

            Console.Write("Napomena (opciono): ");
            string? napomena = Console.ReadLine();

            try
            {
                var e = evidencijaVinaServis.ZabeleziProizvodnju(
                    fid,
                    naziv ?? "",
                    brojFlasa,
                    zapremina,
                    napomena ?? ""
                );

                Console.WriteLine($"Zabeleženo: {e.NazivVina} | {e.UkupnoLitara} L ({e.BrojFlasa} × {e.ZapreminaFlaseLitara} L)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
            }
        }

        private void PregledSvih()
        {
            var lista = evidencijaVinaServis.PregledSvihEvidencija();

            Console.WriteLine("\n--- Svi gotovi proizvodi ---");
            int i = 0;
            foreach (var e in lista)
            {
                i++;
                Console.WriteLine($"{i}) {e.DatumVreme} | {e.NazivVina} | {e.UkupnoLitara} L | Fermentacija={e.FermentacijaId}");
            }

            if (i == 0)
                Console.WriteLine("Nema zabeleženih proizvoda.");
        }

        private void PregledPoFermentaciji()
        {
            Console.Write("Unesi FermentacijaId (GUID): ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid fid))
            {
                Console.WriteLine("Neispravan GUID.");
                return;
            }

            var lista = evidencijaVinaServis.PregledEvidencijaZaFermentaciju(fid);

            Console.WriteLine("\n--- Gotovi proizvodi za fermentaciju ---");
            int i = 0;
            foreach (var e in lista)
            {
                i++;
                Console.WriteLine($"{i}) {e.DatumVreme} | {e.NazivVina} | {e.UkupnoLitara} L ({e.BrojFlasa} × {e.ZapreminaFlaseLitara} L)");
            }

            if (i == 0)
                Console.WriteLine("Nema zabeleženih proizvoda za ovu fermentaciju.");
        }
    }
}
