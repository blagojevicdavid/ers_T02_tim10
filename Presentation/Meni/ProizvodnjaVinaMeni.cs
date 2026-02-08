using System;
using System.Collections.Generic;
using Domain.Modeli;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class ProizvodnjaVinaMeni
    {
        private readonly IEvidencijaProizvodnjeVinaServis evidencijaVinaServis;
       // private readonly PaleteMeni paleteMeni;
        private readonly PakovanjeMeni pakovanjeMeni;

        public ProizvodnjaVinaMeni(
            IEvidencijaProizvodnjeVinaServis evidencijaVinaServis,
            //PaleteMeni paleteMeni,
            PakovanjeMeni pakovanjeMeni)
        {
            this.evidencijaVinaServis = evidencijaVinaServis;
            //this.paleteMeni = paleteMeni;
            this.pakovanjeMeni = pakovanjeMeni;
        }

        public void Prikazi()
        {
            bool izlaz = false;

            while (!izlaz)
            {
                Console.Clear();
                Console.WriteLine("=== PROIZVODNJA VINA ===");
                Console.WriteLine("1) Pregled svih evidencija proizvodnje");
                Console.WriteLine("2) Pregled evidencija za fermentaciju");
                Console.WriteLine("3) Zabeleži proizvodnju (kreiraj evidenciju)");
                //Console.WriteLine("4) Pregled paleta");
                //Console.WriteLine("5) Priprema vina za prodaju (pakovanje + slanje)");
                Console.WriteLine("0) Nazad");
                Console.Write("Izbor: ");

                string izbor = Console.ReadLine() ?? "";

                switch (izbor)
                {
                    case "1":
                        PrikaziSveEvidencije();
                        break;

                    case "2":
                        PrikaziEvidencijeZaFermentaciju();
                        break;

                    case "3":
                        ZabeleziProizvodnju();
                        break;

                        /*
                    case "4":
                        paleteMeni.Prikazi();
                        break;
                    case "5":
                        pakovanjeMeni.Prikazi();
                        break;
                        */
                    case "0":
                        izlaz = true;
                        break;
                }
            }
        }

        private void PrikaziSveEvidencije()
        {
            Console.Clear();
            Console.WriteLine("--- SVE EVIDENCIJE PROIZVODNJE ---");

            IEnumerable<EvidencijaProizvodnjeVina> evidencije = evidencijaVinaServis.PregledSvihEvidencija();

            int brojac = 0;
            foreach (var e in evidencije)
            {
                brojac++;
                Console.WriteLine($"{brojac}) {FormatEvidencija(e)}");
            }

            if (brojac == 0)
                Console.WriteLine("Nema evidentiranih proizvodnji.");

            Console.WriteLine("\nPritisni ENTER...");
            Console.ReadLine();
        }

        private void PrikaziEvidencijeZaFermentaciju()
        {
            Console.Clear();
            Console.WriteLine("--- EVIDENCIJE ZA FERMENTACIJU ---");

            Console.Write("Unesi FermentacijaId (GUID): ");
            string unos = Console.ReadLine() ?? "";

            if (!Guid.TryParse(unos, out Guid fermentacijaId))
            {
                Console.WriteLine("Neispravan GUID.");
                Console.WriteLine("\nPritisni ENTER...");
                Console.ReadLine();
                return;
            }

            IEnumerable<EvidencijaProizvodnjeVina> evidencije = evidencijaVinaServis.PregledEvidencijaZaFermentaciju(fermentacijaId);

            int brojac = 0;
            foreach (var e in evidencije)
            {
                brojac++;
                Console.WriteLine($"{brojac}) {FormatEvidencija(e)}");
            }

            if (brojac == 0)
                Console.WriteLine("Nema evidencija za datu fermentaciju.");

            Console.WriteLine("\nPritisni ENTER...");
            Console.ReadLine();
        }

        private void ZabeleziProizvodnju()
        {
            Console.Clear();
            Console.WriteLine("--- ZABELEŽI PROIZVODNJU ---");

            Console.Write("FermentacijaId (GUID): ");
            string ferStr = Console.ReadLine() ?? "";
            if (!Guid.TryParse(ferStr, out Guid fermentacijaId))
            {
                Console.WriteLine("Neispravan GUID.");
                Console.WriteLine("\nPritisni ENTER...");
                Console.ReadLine();
                return;
            }

            Console.Write("Naziv vina: ");
            string naziv = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(naziv))
            {
                Console.WriteLine("Naziv ne sme biti prazan.");
                Console.WriteLine("\nPritisni ENTER...");
                Console.ReadLine();
                return;
            }

            Console.Write("Broj flasa: ");
            if (!int.TryParse(Console.ReadLine(), out int brojFlasa) || brojFlasa <= 0)
            {
                Console.WriteLine("Broj flasa mora biti > 0.");
                Console.WriteLine("\nPritisni ENTER...");
                Console.ReadLine();
                return;
            }

            Console.Write("Zapremina flase (litara, npr 0.75): ");
            if (!double.TryParse(Console.ReadLine(), out double zapremina) || zapremina <= 0)
            {
                Console.WriteLine("Zapremina mora biti > 0.");
                Console.WriteLine("\nPritisni ENTER...");
                Console.ReadLine();
                return;
            }

            Console.Write("Napomena (opciono): ");
            string napomena = Console.ReadLine() ?? "";

            try
            {
                var e = evidencijaVinaServis.ZabeleziProizvodnju(
                    fermentacijaId,
                    naziv,
                    brojFlasa,
                    zapremina,
                    napomena
                );

                Console.WriteLine("\nProizvodnja je evidentirana:");
                Console.WriteLine(FormatEvidencija(e));
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nGreška: " + ex.Message);
            }

            Console.WriteLine("\nPritisni ENTER...");
            Console.ReadLine();
        }

        private static string FormatEvidencija(EvidencijaProizvodnjeVina e)
        {
            if (e == null) return "(null evidencija)";

            
            return
                $"Id={e.Id} | FermentacijaId={e.FermentacijaId} | Vino={e.NazivVina} | " +
                $"Flase={e.BrojFlasa} | Zapremina={e.ZapreminaFlaseLitara}L | Ukupno={e.UkupnoLitara}L | " +
                $"Datum={e.DatumVreme}";
        }
    }
}
