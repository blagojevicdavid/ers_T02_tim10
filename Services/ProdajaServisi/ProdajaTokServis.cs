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

            Vino vinoTip = PronadjiVinoIliNapraviTip(nazivVina, kategorija, zapremina);
            if (vinoTip == null || vinoTip.Naziv == null || vinoTip.Naziv.Trim().Length == 0)
                return Guid.Empty;

            int dostupno = PrebrojDostupno(vinoTip);
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

                    dostupno = PrebrojDostupno(vinoTip);
                    if (dostupno < 0)
                        return Guid.Empty;

                    potrebno = brojFlasa - dostupno;
                }
            }

            var vinoIdsZaKupca = UzmiSaStanja(vinoTip, brojFlasa);
            if (vinoIdsZaKupca == null || vinoIdsZaKupca.Count != brojFlasa)
                return Guid.Empty;

            Paleta paleta = KreirajPaletuZaKupca(vinoIdsZaKupca, adresaOdredista, vinskiPodrumId);
            if (paleta == null || paleta.Id == Guid.Empty)
                return Guid.Empty;

            paleta.Status = StatusPalete.Otpremljena;

            bool okAzur = _paleteRepo.AzurirajPaletu(paleta);
            if (!okAzur)
                return Guid.Empty;

            decimal cenaPoKomadu = IzracunajCenu(tipProdaje);

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

        private Vino PronadjiVinoIliNapraviTip(string nazivVina, KategorijaVina kategorija, double zapremina)
        {
            string trazeni = nazivVina == null ? string.Empty : nazivVina.Trim();

            var lista = _vinoRepo.PronadjiVinaPoKategoriji(kategorija);
            if (lista != null)
            {
                foreach (var v in lista)
                {
                    if (OdgovaraTipu(v, trazeni, kategorija, zapremina))
                        return v;
                }
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
            if (vinoTip == null)
                return -1;

            var raspakovane = _paleteRepo.PronadjiPaletePoStatusu(StatusPalete.Raspakovana);
            if (raspakovane == null)
                return 0;

            int suma = 0;

            foreach (var p in raspakovane)
            {
                if (p == null || p.VinaIds == null || p.VinaIds.Count == 0)
                    continue;

                foreach (var id in p.VinaIds)
                {
                    Vino v = _vinoRepo.PronadjiVinoPoId(id);
                    if (v == null)
                        continue;

                    if (OdgovaraTipu(v, vinoTip.Naziv, vinoTip.Kategorija, vinoTip.ZapreminaLitara))
                        suma++;
                }
            }

            return suma;
        }

        private List<Guid> UzmiSaStanja(Vino vinoTip, int kolicina)
        {
            if (vinoTip == null || kolicina <= 0)
                return new List<Guid>();

            var raspakovaneEnum = _paleteRepo.PronadjiPaletePoStatusu(StatusPalete.Raspakovana);
            if (raspakovaneEnum == null)
                return new List<Guid>();

            var raspakovane = raspakovaneEnum.ToList();

            int preostalo = kolicina;
            List<Guid> uzeto = new List<Guid>(kolicina);

            foreach (var p in raspakovane)
            {
                if (preostalo <= 0)
                    break;

                if (p == null || p.VinaIds == null || p.VinaIds.Count == 0)
                    continue;

                for (int i = p.VinaIds.Count - 1; i >= 0 && preostalo > 0; i--)
                {
                    Guid id = p.VinaIds[i];

                    Vino v = _vinoRepo.PronadjiVinoPoId(id);
                    if (v == null)
                        continue;

                    if (!OdgovaraTipu(v, vinoTip.Naziv, vinoTip.Kategorija, vinoTip.ZapreminaLitara))
                        continue;

                    uzeto.Add(id);
                    p.VinaIds.RemoveAt(i);
                    preostalo--;
                }

                _paleteRepo.AzurirajPaletu(p);
            }

            if (preostalo > 0)
                return new List<Guid>();

            return uzeto;
        }

        private Paleta KreirajPaletuZaKupca(List<Guid> vinoIds, string adresaOdredista, Guid vinskiPodrumId)
        {
            if (vinoIds == null || vinoIds.Count == 0)
                return new Paleta();

            string adresa = adresaOdredista == null ? string.Empty : adresaOdredista.Trim();
            if (adresa.Length == 0 || vinskiPodrumId == Guid.Empty)
                return new Paleta();

            Paleta paleta = new Paleta
            {
                Id = Guid.NewGuid(),
                Sifra = "PL-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "-" + Guid.NewGuid().ToString().Substring(0, 8),
                AdresaOdredista = adresa,
                VinskiPodrumId = vinskiPodrumId,
                Status = StatusPalete.Upakovana,
                VinaIds = new List<Guid>(vinoIds)
            };

            var sacuvana = _paleteRepo.DodajPaletu(paleta);
            if (sacuvana == null || sacuvana.Id == Guid.Empty)
                return new Paleta();

            return sacuvana;
        }

        private bool OdgovaraTipu(Vino v, string naziv, KategorijaVina kategorija, double zapremina)
        {
            if (v == null)
                return false;

            string n1 = v.Naziv == null ? string.Empty : v.Naziv.Trim();
            string n2 = naziv == null ? string.Empty : naziv.Trim();

            if (!string.Equals(n1, n2, StringComparison.OrdinalIgnoreCase))
                return false;

            if (v.Kategorija != kategorija)
                return false;

            if (Math.Abs(v.ZapreminaLitara - zapremina) > 0.0001)
                return false;

            return true;
        }

        private decimal IzracunajCenu(TipProdaje tipProdaje)
        {
            decimal bazna = 10m;
            return tipProdaje == TipProdaje.Diskont ? bazna * 0.85m : bazna;
        }
    }
}
