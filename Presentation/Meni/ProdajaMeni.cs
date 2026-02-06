using System;
using Domain.Enumeracije;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class ProdajaMeni
    {
        private readonly IProdajaTokServis _prodajaTok;

        public ProdajaMeni(IProdajaTokServis prodajaTok)
        {
            _prodajaTok = prodajaTok ?? throw new ArgumentNullException(nameof(prodajaTok));
        }

        public void Prikazi()
        {
            Console.WriteLine("\n=== PRODAJA VINA ===");

            Console.Write("Unesite naziv vina: ");
            string nazivVina = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nazivVina))
            {
                Console.WriteLine("Naziv vina je obavezan.");
                return;
            }

            Console.WriteLine("Izaberite kategoriju vina:");
            Console.WriteLine("1) Stolno vino");
            Console.WriteLine("2) Kvalitetno vino");
            Console.WriteLine("3) Premijum vino");
            Console.Write("Opcija: ");
            if (!int.TryParse(Console.ReadLine(), out int katOpt) || katOpt < 1 || katOpt > 3)
            {
                Console.WriteLine("Neispravna kategorija.");
                return;
            }
            KategorijaVina kategorija = (KategorijaVina)(katOpt - 1);

            Console.Write("Unesite broj flasa: ");
            if (!int.TryParse(Console.ReadLine(), out int brojFlasa) || brojFlasa <= 0)
            {
                Console.WriteLine("Neispravan broj flasa.");
                return;
            }

            Console.Write("Unesite zapreminu flase u litrima (0.75 ili 1.5): ");
            string zap = (Console.ReadLine() ?? "").Trim();
            zap = zap.Replace(',', '.');
            if (!double.TryParse(zap, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double zapremina) ||
                (Math.Abs(zapremina - 0.75) > 0.0001 && Math.Abs(zapremina - 1.5) > 0.0001))
            {
                Console.WriteLine("Neispravna zapremina.");
                return;
            }

            Console.WriteLine("Izaberite tip prodaje:");
            Console.WriteLine("1) Restoranska prodaja");
            Console.WriteLine("2) Diskont pica");
            Console.Write("Opcija: ");
            if (!int.TryParse(Console.ReadLine(), out int tipOpt) || tipOpt < 1 || tipOpt > 2)
            {
                Console.WriteLine("Neispravan tip prodaje.");
                return;
            }
            TipProdaje tipProdaje = tipOpt switch
            {
                1 => TipProdaje.Restoranska,
                2 => TipProdaje.Diskont,
                _ => throw new InvalidOperationException("Nepoznat tip prodaje.")
            };

            Console.WriteLine("Izaberite način placanja:");
            Console.WriteLine("1) Gotovina");
            Console.WriteLine("2) Predracun");
            Console.WriteLine("3) Gotovinski racun");
            Console.Write("Opcija: ");
            if (!int.TryParse(Console.ReadLine(), out int placOpt) || placOpt < 1 || placOpt > 3)
            {
                Console.WriteLine("Neispravan način placanja.");
                return;
            }
            NacinPlacanja nacinPlacanja = placOpt switch
            {
                1 => NacinPlacanja.Gotovina,
                2 => NacinPlacanja.Predracun,
                3 => NacinPlacanja.GotovinskiRacun,
                _ => throw new InvalidOperationException("Nepoznat način placanja.")
            };

            Console.Write("Unesite adresu odredista: ");
            string adresa = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(adresa))
            {
                Console.WriteLine("Adresa odredista je obavezna.");
                return;
            }

            Console.Write("Unesite kupca (naziv/adresa): ");
            string kupac = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(kupac))
            {
                Console.WriteLine("Kupac je obavezan.");
                return;
            }

            Guid vinskiPodrumId = Guid.Empty;

            try
            {
                Guid fakturaId = _prodajaTok.IzvrsiProdaju(
                    nazivVina,
                    kategorija,
                    brojFlasa,
                    zapremina,
                    tipProdaje,
                    nacinPlacanja,
                    adresa,
                    vinskiPodrumId,
                    kupac
                );

                Console.WriteLine($"Prodaja uspješna! Kreirana faktura (Id): {fakturaId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
            }

            Console.WriteLine("\nPritisni ENTER za nastavak...");
            Console.ReadLine();
        }
    }
}
