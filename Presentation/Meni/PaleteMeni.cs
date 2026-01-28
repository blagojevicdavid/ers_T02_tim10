using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;

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
<<<<<<< HEAD
            Console.WriteLine("\n--- SLANJE PALETA U PODRUM---");
=======
            bool nazad = false;

            while (!nazad)
            {
                Console.WriteLine("\n--- PALETE ---");
                Console.WriteLine("1) Kreiranje nove palete");
                Console.WriteLine("2) Slanje paleta u podrum");
                Console.WriteLine("3) Zapakuj vino u paletu");
                Console.WriteLine("0) Nazad");
                Console.Write("Izbor: ");

                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        KreirajNovuPaletu();
                        break;

                    case "2":
                        PosaljiPaleteUVinskiPodrum();
                        break;

                    case "3":
                        ZapakujVinoUPaletu();
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


        private void KreirajNovuPaletu()
        {
            Console.WriteLine("\n--- KREIRANJE NOVE PALETE---");
            Console.Write("Unesi adresu odredišta: ");
            string? adresa = Console.ReadLine();

            try
            {
                var paleta = paleteServis.KreirajNovuPaletu(adresa ?? string.Empty);

                Console.WriteLine("Paleta je uspešno kreirana!");
                Console.WriteLine($"Šifra: {paleta.Sifra}");
                Console.WriteLine($"ID: {paleta.Id}");
                Console.WriteLine($"Adresa: {paleta.AdresaOdredista}");
                Console.WriteLine($"Status: {paleta.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
            }

            Console.WriteLine("Pritisni ENTER za nastavak...");
            Console.ReadLine();
        }

        private void PosaljiPaleteUVinskiPodrum()
        {
            Console.WriteLine("\n--- SLANJE PALETA U PODRUM ---");
>>>>>>> 7d356f703eacf9bf164455f1f8b479e0d8615f72

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

        private void ZapakujVinoUPaletu()
        {
            Console.Write("Unesi PaletaId (GUID): ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid paletaId))
            {
                Console.WriteLine("Neispravan GUID.");
                return;
            }

            Console.Write("Unesi ID proizvedenog vina (EvidencijaProizvodnjeVinaId) (GUID): ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid evidencijaId))
            {
                Console.WriteLine("Neispravan GUID.");
                return;
            }

            bool ok = paleteServis.DodajProizvedenoVinoNaPaletu(paletaId, evidencijaId);
            Console.WriteLine(ok ? "Vino je zapakovano u paletu." : "Neuspešno (paleta ili evidencija ne postoji / već dodato).");
        }



    }
}
