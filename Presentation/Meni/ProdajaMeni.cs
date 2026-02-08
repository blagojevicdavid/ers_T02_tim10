using Domain.Enumeracije;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Services.SkladistenjeServisi;
using System;
using System.Globalization;
using System.Linq;

namespace Presentation.Meni
{
    public class ProdajaMeni
    {
        private readonly IProdajaTokServis _prodajaTok;
        private readonly ISkladistenjeServis _skladistenjeServis;
        private readonly IVinskiPodrumRepozitorijum _vinskiPodrumRepo;

        public ProdajaMeni( IProdajaTokServis prodajaTok, ISkladistenjeServis skladistenjeServis, IVinskiPodrumRepozitorijum vinskiPodrumRepo)
        {
            _prodajaTok = prodajaTok;
            _skladistenjeServis = skladistenjeServis;
            _vinskiPodrumRepo = vinskiPodrumRepo;
        }

        public void Prikazi()
        {
            Console.WriteLine("\n=== PRODAJA VINA ===");

            Console.Write("Unesite naziv vina: ");
            var nazivVina = Console.ReadLine();
            if (nazivVina == null)
            {
                nazivVina = "";
            }
            nazivVina = nazivVina.Trim();
            if (string.IsNullOrWhiteSpace(nazivVina))
            {
                Console.WriteLine("Naziv vina je obavezan.");
                Pauza();
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
                Pauza();
                return;
            }
            KategorijaVina kategorija = (KategorijaVina)(katOpt - 1);

            Console.Write("Unesite broj flasa: ");
            if (!int.TryParse(Console.ReadLine(), out int brojFlasa) || brojFlasa <= 0)
            {
                Console.WriteLine("Neispravan broj flasa.");
                Pauza();
                return;
            }

            Console.Write("Unesite zapreminu flase u litrima (0.75 ili 1.5): ");
            var zapTxt = Console.ReadLine();
            if (zapTxt == null)
            {
                zapTxt = "";
            }
            zapTxt = zapTxt.Trim();
            zapTxt = zapTxt.Replace(',', '.');
            ;
            if (!double.TryParse(zapTxt, NumberStyles.Float, CultureInfo.InvariantCulture, out double zapremina) ||
                (Math.Abs(zapremina - 0.75) > 0.0001 && Math.Abs(zapremina - 1.5) > 0.0001))
            {
                Console.WriteLine("Neispravna zapremina.");
                Pauza();
                return;
            }

            Console.WriteLine("Izaberite tip prodaje:");
            Console.WriteLine("1) Restoranska prodaja");
            Console.WriteLine("2) Diskont pica");
            Console.Write("Opcija: ");
            if (!int.TryParse(Console.ReadLine(), out int tipOpt) || tipOpt < 1 || tipOpt > 2)
            {
                Console.WriteLine("Neispravan tip prodaje.");
                Pauza();
                return;
            }
            TipProdaje tipProdaje;

            if (tipOpt == 1)
            {
                tipProdaje = TipProdaje.Restoranska;
            }
            else
            {
                tipProdaje = TipProdaje.Diskont;
            }


            Console.WriteLine("Izaberite način placanja:");
            Console.WriteLine("1) Gotovina");
            Console.WriteLine("2) Predracun");
            Console.WriteLine("3) Gotovinski racun");
            Console.Write("Opcija: ");
            if (!int.TryParse(Console.ReadLine(), out int placOpt) || placOpt < 1 || placOpt > 3)
            {
                Console.WriteLine("Neispravan način placanja.");
                Pauza();
                return;
            }

            NacinPlacanja nacinPlacanja;
            if (placOpt == 1) nacinPlacanja = NacinPlacanja.Gotovina;
            else if (placOpt == 2) nacinPlacanja = NacinPlacanja.Predracun;
            else nacinPlacanja = NacinPlacanja.GotovinskiRacun;
            Console.Write("Unesite adresu odredista: ");
            var adresa = Console.ReadLine();
            if (adresa == null)
            {
                adresa = "";
            }

            adresa = adresa.Trim();

            if (adresa == "")
            {
                Console.WriteLine("Adresa odredista je obavezna.");
                Pauza();
                return;
            }

            Console.Write("Unesite kupca (naziv/adresa): ");
            var kupac = Console.ReadLine();
            if (kupac == null)
            {
                kupac = "";
            }

            kupac = kupac.Trim();

            if (kupac == "")
            {
                Console.WriteLine("Kupac je obavezan.");
                Pauza();
                return;
            }


            Guid vinskiPodrumId = PreuzmiIliPostaviPodrum();
            if (vinskiPodrumId == Guid.Empty)
            {
                Console.WriteLine("Nema nijednog podruma u sistemu. Ne mogu izvršiti prodaju.");
                Pauza();
                return;
            }

            Guid fakturaId = _prodajaTok.IzvrsiProdaju(nazivVina,kategorija,brojFlasa,zapremina,tipProdaje,nacinPlacanja,adresa,vinskiPodrumId,kupac );

            if (fakturaId == Guid.Empty)
            {
                Console.WriteLine("Prodaja NIJE izvršena. Provjerite da li vino postoji u katalogu i da li su podaci ispravni.");
                Pauza();
                return;
            }

            Console.WriteLine($"Prodaja uspješna! Kreirana faktura (Id): {fakturaId}");
            Pauza();
        }

        private Guid PreuzmiIliPostaviPodrum()
        {
            if (_skladistenjeServis == null || _vinskiPodrumRepo == null)
                return Guid.Empty;

            Guid id = Guid.Empty;
            var nacin = _skladistenjeServis.PreuzmiNacinSkladistenja();

            if (nacin == NacinSkladistenja.LokalniPodrum)
                id = _skladistenjeServis.PreuzmiLokalniPodrum();
            else
                id = _skladistenjeServis.PreuzmiVinskiPodrum();

            if (id != Guid.Empty)
                return id;

            var prvi = _vinskiPodrumRepo.SviVinskiPodrumi().FirstOrDefault();
            if (prvi == null)
                return Guid.Empty;

            id = prvi.Id;

            if (nacin == NacinSkladistenja.LokalniPodrum)
                _skladistenjeServis.PostaviLokalniPodrum(id);
            else
                _skladistenjeServis.PostaviVinskiPodrum(id);

            return id;
        }

        private void Pauza()
        {
            Console.WriteLine("\nPritisni ENTER za nastavak...");
            Console.ReadLine();
        }
    }
}
