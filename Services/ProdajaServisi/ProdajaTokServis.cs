using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.ProdajaServisi
{
    public class ProdajaTokServis : IProdajaTokServis
    {
        private readonly IPakovanjeServis _pakovanje;
        private readonly IProdajaServis _prodaja;
        private readonly ILoggerServis _logger;

        private readonly IVinoRepozitorijum _vinoRepo;
        private readonly IPaleteRepozitorijum _paleteRepo;
        private readonly ISkladistenjeServis _skladistenje;

        public ProdajaTokServis(
            IPakovanjeServis pakovanje,
            ISkladistenjeServis skladistenje,
            IProdajaServis prodaja,
            ILoggerServis logger,
            IVinoRepozitorijum vinoRepo,
            IPaleteRepozitorijum paleteRepo)
        {
            _pakovanje = pakovanje;
            _skladistenje = skladistenje;
            _prodaja = prodaja;
            _logger = logger;
            _vinoRepo = vinoRepo;
            _paleteRepo = paleteRepo;
        }

        public Guid IzvrsiProdaju(
            string nazivVina,
            KategorijaVina kategorija,
            int brojFlasa,
            double zapremina,
            TipProdaje tipProdaje,
            NacinPlacanja nacinPlacanja,
            string adresaOdredista,
            Guid vinskiPodrumId,
            string kupac)
        {
            if (string.IsNullOrWhiteSpace(nazivVina)) throw new ArgumentException("Naziv vina je obavezan.");
            if (brojFlasa <= 0) throw new ArgumentException("Broj flaša mora biti > 0.");
            if (Math.Abs(zapremina - 0.75) > 0.0001 && Math.Abs(zapremina - 1.5) > 0.0001)
                throw new ArgumentException("Zapremina može biti samo 0.75 ili 1.5.");

            Vino vinoTip = PronadjiVinoIliNapraviTip(nazivVina, kategorija, zapremina);

            int dostupno = PrebrojDostupno(vinoTip);

            if (dostupno < brojFlasa)
            {
                int potrebno = brojFlasa - dostupno;

                while (potrebno > 0)
                {
                    var (ok, _) = _pakovanje.PosaljiPrvuDostupnuUpakovanuPaletu(
                        vinoTip.Naziv,
                        vinoTip.Kategorija,
                        potrebno,
                        vinoTip.ZapreminaLitara,
                        adresaOdredista,
                        vinskiPodrumId
                    );

                    if (!ok)
                        throw new InvalidOperationException("Nije moguće proizvesti i dopuniti stanje.");

                    var isporucene = _skladistenje.IsporuciPaleteZaProdaju(1);
                    if (isporucene == null || !isporucene.Any())
                        throw new InvalidOperationException("Paleta proizvedena ali nije mogla biti raspakovana.");

                    dostupno = PrebrojDostupno(vinoTip);
                    potrebno = brojFlasa - dostupno;
                }
            }

            var vinoIdsZaKupca = UzmiSaStanja(vinoTip, brojFlasa);

            Paleta paleta = KreirajPaletuZaKupca(vinoIdsZaKupca, adresaOdredista, vinskiPodrumId);
            paleta.Status = StatusPalete.Otpremljena;
            _paleteRepo.AzurirajPaletu(paleta);

            decimal cenaPoKomadu = IzracunajCenu(tipProdaje);

            Guid fakturaId = _prodaja.IsporuciVinoKupcu(
                paleta.Id,
                kupac,
                cenaPoKomadu,
                tipProdaje,
                nacinPlacanja
            );

            _logger.Evidentiraj(
                TipEvidencije.INFO,
                $"[PRODAJA TOK] Prodaja OK. Vino={vinoTip.Naziv}, Kolicina={brojFlasa}, Paleta={paleta.Sifra}, Faktura={fakturaId}, Tip={tipProdaje}, Placanje={nacinPlacanja}"
            );

            return fakturaId;
        }

        private Vino PronadjiVinoIliNapraviTip(string nazivVina, KategorijaVina kategorija, double zapremina)
        {
            string trazeni = (nazivVina ?? string.Empty).Trim();

            foreach (var v in _vinoRepo.PronadjiVinaPoKategoriji(kategorija) ?? Enumerable.Empty<Vino>())
            {
                if (OdgovaraTipu(v, trazeni, kategorija, zapremina))
                    return v;
            }

            return new Vino
            {
                Id = Guid.Empty,
                Naziv = trazeni,
                Kategorija = kategorija,
                ZapreminaLitara = zapremina
            };
        }

        private int PrebrojDostupno(Vino vinoTip)
        {
            var raspakovane = _paleteRepo.PronadjiPaletePoStatusu(StatusPalete.Raspakovana) ?? Enumerable.Empty<Paleta>();

            int suma = 0;

            foreach (var p in raspakovane)
            {
                if (p?.VinaIds == null || p.VinaIds.Count == 0) continue;

                foreach (var id in p.VinaIds)
                {
                    Vino v;
                    try
                    {
                        v = _vinoRepo.PronadjiVinoPoId(id);
                    }
                    catch
                    {
                        continue;
                    }

                    if (OdgovaraTipu(v, vinoTip.Naziv, vinoTip.Kategorija, vinoTip.ZapreminaLitara))
                        suma++;
                }
            }

            return suma;
        }

        private List<Guid> UzmiSaStanja(Vino vinoTip, int kolicina)
        {
            var raspakovane = (_paleteRepo.PronadjiPaletePoStatusu(StatusPalete.Raspakovana) ?? Enumerable.Empty<Paleta>())
                .ToList();

            int preostalo = kolicina;
            List<Guid> uzeto = new List<Guid>(kolicina);

            foreach (var p in raspakovane)
            {
                if (preostalo <= 0) break;
                if (p?.VinaIds == null || p.VinaIds.Count == 0) continue;

                for (int i = p.VinaIds.Count - 1; i >= 0 && preostalo > 0; i--)
                {
                    Guid id = p.VinaIds[i];

                    Vino v;
                    try
                    {
                        v = _vinoRepo.PronadjiVinoPoId(id);
                    }
                    catch
                    {
                        continue;
                    }

                    if (!OdgovaraTipu(v, vinoTip.Naziv, vinoTip.Kategorija, vinoTip.ZapreminaLitara))
                        continue;

                    uzeto.Add(id);
                    p.VinaIds.RemoveAt(i);
                    preostalo--;
                }

                _paleteRepo.AzurirajPaletu(p);
            }

            if (preostalo > 0)
                throw new InvalidOperationException("Greška pri skidanju sa stanja (nedovoljno vina u raspakovanim paletama).");

            return uzeto;
        }

        private Paleta KreirajPaletuZaKupca(List<Guid> vinoIds, string adresaOdredista, Guid vinskiPodrumId)
        {
            if (vinoIds == null || vinoIds.Count == 0)
                throw new InvalidOperationException("Ne mogu kreirati paletu bez vina.");

            Paleta paleta = new Paleta
            {
                Id = Guid.NewGuid(),
                Sifra = $"PL-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}",
                AdresaOdredista = (adresaOdredista ?? string.Empty).Trim(),
                VinskiPodrumId = vinskiPodrumId,
                Status = StatusPalete.Upakovana,
                VinaIds = new List<Guid>(vinoIds)
            };

            var sacuvana = _paleteRepo.DodajPaletu(paleta);
            if (sacuvana == null || sacuvana.Id == Guid.Empty)
                throw new InvalidOperationException("Neuspješno čuvanje palete za kupca.");

            return sacuvana;
        }

        private bool OdgovaraTipu(Vino v, string naziv, KategorijaVina kategorija, double zapremina)
        {
            if (v == null) return false;

            string n1 = (v.Naziv ?? string.Empty).Trim();
            string n2 = (naziv ?? string.Empty).Trim();

            if (!string.Equals(n1, n2, StringComparison.OrdinalIgnoreCase)) return false;
            if (v.Kategorija != kategorija) return false;
            if (Math.Abs(v.ZapreminaLitara - zapremina) > 0.0001) return false;

            return true;
        }

        private decimal IzracunajCenu(TipProdaje tipProdaje)
        {
            decimal bazna = 10m;
            return tipProdaje == TipProdaje.Diskont ? bazna * 0.85m : bazna;
        }
    }
}
