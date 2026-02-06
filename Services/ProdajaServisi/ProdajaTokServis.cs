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

        public ProdajaTokServis(
            IPakovanjeServis pakovanje,
            IProdajaServis prodaja,
            ILoggerServis logger,
            IVinoRepozitorijum vinoRepo,
            IPaleteRepozitorijum paleteRepo)
        {
            _pakovanje = pakovanje ?? throw new ArgumentNullException(nameof(pakovanje));
            _prodaja = prodaja ?? throw new ArgumentNullException(nameof(prodaja));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _vinoRepo = vinoRepo ?? throw new ArgumentNullException(nameof(vinoRepo));
            _paleteRepo = paleteRepo ?? throw new ArgumentNullException(nameof(paleteRepo));
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

            Vino vino = PronadjiVino(nazivVina, kategorija, zapremina);
            int dostupno = PrebrojDostupno(vino.Id);

            if (dostupno < brojFlasa)
                throw new InvalidOperationException($"Nema dovoljno flaša na stanju. Dostupno: {dostupno}, traženo: {brojFlasa}.");

            SkiniSaStanja(vino.Id, brojFlasa);

            var (ok, paleta) = _pakovanje.UpakujVinaUPaletu(
                vino.Naziv,
                kategorija,
                brojFlasa,
                zapremina,
                adresaOdredista,
                vinskiPodrumId
            );

            if (!ok || paleta == null || paleta.Id == Guid.Empty)
                throw new InvalidOperationException("Neuspešno pakovanje palete.");

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
                $"[PRODAJA TOK] Prodaja OK. Vino={vino.Naziv}, Kolicina={brojFlasa}, Paleta={paleta.Sifra}, Faktura={fakturaId}, Tip={tipProdaje}, Placanje={nacinPlacanja}"
            );

            return fakturaId;
        }

        private Vino PronadjiVino(string nazivVina, KategorijaVina kategorija, double zapremina)
        {
            string trazeni = nazivVina.Trim();

            foreach (var v in _vinoRepo.PronadjiVinaPoKategoriji(kategorija) ?? Enumerable.Empty<Vino>())
            {
                if (string.Equals((v.Naziv ?? "").Trim(), trazeni, StringComparison.OrdinalIgnoreCase) &&
                    Math.Abs(v.ZapreminaLitara - zapremina) < 0.0001)
                {
                    return v;
                }
            }

            throw new InvalidOperationException("Vino ne postoji (naziv/kategorija/zapremina se ne poklapaju).");
        }

        private int PrebrojDostupno(Guid vinoId)
        {
            var raspakovane = _paleteRepo.PronadjiPaletePoStatusu(StatusPalete.Raspakovana) ?? Enumerable.Empty<Paleta>();

            int suma = 0;
            foreach (var p in raspakovane)
            {
                if (p?.VinaIds == null) continue;
                suma += p.VinaIds.Count(id => id == vinoId);
            }

            return suma;
        }

        private void SkiniSaStanja(Guid vinoId, int kolicina)
        {
            var raspakovane = (_paleteRepo.PronadjiPaletePoStatusu(StatusPalete.Raspakovana) ?? Enumerable.Empty<Paleta>())
                .ToList();

            int preostalo = kolicina;

            foreach (var p in raspakovane)
            {
                if (preostalo <= 0) break;
                if (p?.VinaIds == null || p.VinaIds.Count == 0) continue;

                int imaUOvoj = p.VinaIds.Count(id => id == vinoId);
                if (imaUOvoj == 0) continue;

                int skidam = Math.Min(preostalo, imaUOvoj);

                for (int i = 0; i < skidam; i++)
                {
                    int idx = p.VinaIds.FindIndex(id => id == vinoId);
                    if (idx >= 0) p.VinaIds.RemoveAt(idx);
                }

                _paleteRepo.AzurirajPaletu(p);
                preostalo -= skidam;
            }

            if (preostalo > 0)
                throw new InvalidOperationException("Greška pri skidanju sa stanja (nedovoljno vina u raspakovanim paletama).");
        }

        private decimal IzracunajCenu(TipProdaje tipProdaje)
        {
            decimal bazna = 10m;
            return tipProdaje == TipProdaje.Diskont ? bazna * 0.85m : bazna;
        }
    }
}
