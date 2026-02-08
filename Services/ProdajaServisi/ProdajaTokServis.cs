using System;
using System.Linq;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Services.ProdajaServisi.PomocneMetode;

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

        private readonly ProdajaStanjeServis _stanjeServis;
        private readonly ProdajaPaletaFabrika _paletaFabrika;
        private readonly ProdajaCenovnik _cenovnik;

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

            _stanjeServis = new ProdajaStanjeServis(_vinoRepo, _paleteRepo, _logger);
            _paletaFabrika = new ProdajaPaletaFabrika(_paleteRepo, _logger);
            _cenovnik = new ProdajaCenovnik();
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
            if (_pakovanje == null || _skladistenje == null || _prodaja == null || _logger == null || _vinoRepo == null || _paleteRepo == null)
                return Guid.Empty;

            if (nazivVina == null || nazivVina.Trim().Length == 0)
                return Guid.Empty;

            if (kupac == null || kupac.Trim().Length == 0)
                return Guid.Empty;

            if (adresaOdredista == null || adresaOdredista.Trim().Length == 0)
                return Guid.Empty;

            if (vinskiPodrumId == Guid.Empty)
                return Guid.Empty;

            if (brojFlasa <= 0)
                return Guid.Empty;

            if (Math.Abs(zapremina - 0.75) > 0.0001 && Math.Abs(zapremina - 1.5) > 0.0001)
                return Guid.Empty;

            Vino vinoTip;
            if (!TryPronadjiVinoTip(nazivVina, kategorija, zapremina, out vinoTip))
            {
                _logger.Evidentiraj(TipEvidencije.ERROR,
                    $"[PRODAJA TOK] Vino ne postoji: Naziv={nazivVina}, Kat={kategorija}, Zap={zapremina}");
                return Guid.Empty;
            }

            if (vinoTip == null || vinoTip.Naziv == null || vinoTip.Naziv.Trim().Length == 0)
                return Guid.Empty;

            int dostupno = _stanjeServis.PrebrojDostupno(vinoTip);
            if (dostupno < 0)
                return Guid.Empty;

            if (dostupno < brojFlasa)
            {
                int potrebno = brojFlasa - dostupno;

                while (potrebno > 0)
                {
                    var rezultat = _pakovanje.PosaljiPrvuDostupnuUpakovanuPaletu(
                        vinoTip.Naziv,
                        vinoTip.Kategorija,
                        potrebno,
                        vinoTip.ZapreminaLitara,
                        adresaOdredista,
                        vinskiPodrumId
                    );

                    if (!rezultat.Item1)
                        return Guid.Empty;

                    var isporucene = _skladistenje.IsporuciPaleteZaProdaju(1);
                    if (isporucene == null || !isporucene.Any())
                        return Guid.Empty;

                    dostupno = _stanjeServis.PrebrojDostupno(vinoTip);
                    if (dostupno < 0)
                        return Guid.Empty;

                    potrebno = brojFlasa - dostupno;
                }
            }

            var vinoIdsZaKupca = _stanjeServis.UzmiSaStanja(vinoTip, brojFlasa);
            if (vinoIdsZaKupca == null || vinoIdsZaKupca.Count != brojFlasa)
                return Guid.Empty;

            Paleta paleta = _paletaFabrika.KreirajPaletuZaKupca(vinoIdsZaKupca, adresaOdredista, vinskiPodrumId);
            if (paleta == null || paleta.Id == Guid.Empty)
                return Guid.Empty;

            paleta.Status = StatusPalete.Otpremljena;

            bool okAzur = _paleteRepo.AzurirajPaletu(paleta);
            if (!okAzur)
                return Guid.Empty;

            decimal cenaPoKomadu = _cenovnik.IzracunajCenu(tipProdaje);

            Guid fakturaId = _prodaja.IsporuciVinoKupcu(
                paleta.Id,
                kupac,
                cenaPoKomadu,
                tipProdaje,
                nacinPlacanja
            );

            if (fakturaId == Guid.Empty)
                return Guid.Empty;

            _logger.Evidentiraj(
                TipEvidencije.INFO,
                "[PRODAJA TOK] Prodaja OK. Vino=" + vinoTip.Naziv +
                ", Kolicina=" + brojFlasa +
                ", Paleta=" + paleta.Sifra +
                ", Faktura=" + fakturaId +
                ", Tip=" + tipProdaje +
                ", Placanje=" + nacinPlacanja
            );

            return fakturaId;
        }

        //ostala ovde zbog _vinoRepo
        private bool TryPronadjiVinoTip(string nazivVina, KategorijaVina kategorija, double zapremina, out Vino vino)
        {
            vino = new Vino();

            string trazeni = string.Empty;
            if (!string.IsNullOrWhiteSpace(nazivVina))
                trazeni = nazivVina.Trim();

            var lista = _vinoRepo.PronadjiVinaPoKategoriji(kategorija);
            if (lista == null)
                return false;

            foreach (var v in lista)
            {
                if (_stanjeServis.OdgovaraTipu(v, trazeni, kategorija, zapremina))
                {
                    vino = v;
                    return true;
                }
            }
            return false;
        }
    }
}
