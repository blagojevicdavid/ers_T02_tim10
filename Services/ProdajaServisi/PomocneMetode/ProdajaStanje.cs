using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.ProdajaServisi.PomocneMetode
{
    internal class ProdajaStanjeServis
    {
        private readonly IVinoRepozitorijum _vinoRepo;
        private readonly IPaleteRepozitorijum _paleteRepo;
        private readonly ILoggerServis _logger;

        public ProdajaStanjeServis(IVinoRepozitorijum vinoRepo, IPaleteRepozitorijum paleteRepo, ILoggerServis logger)
        {
            _vinoRepo = vinoRepo;
            _paleteRepo = paleteRepo;
            _logger = logger;
        }

        public int PrebrojDostupno(Vino vinoTip)
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
                    Vino v;
                    if (!_vinoRepo.PronadjiVinoPoId(id, out v))
                        continue;

                    if (OdgovaraTipu(v, vinoTip.Naziv, vinoTip.Kategorija, vinoTip.ZapreminaLitara))
                        suma++;
                }
            }

            return suma;
        }

        public List<Guid> UzmiSaStanja(Vino vinoTip, int kolicina)
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

                    Vino v;
                    if (!_vinoRepo.PronadjiVinoPoId(id, out v))
                        continue;

                    if (!OdgovaraTipu(v, vinoTip.Naziv, vinoTip.Kategorija, vinoTip.ZapreminaLitara))
                        continue;

                    uzeto.Add(id);
                    p.VinaIds.RemoveAt(i);
                    preostalo--;
                }

                bool ok = _paleteRepo.AzurirajPaletu(p);
                if (!ok)
                {
                    _logger.Evidentiraj(TipEvidencije.ERROR, $"[PRODAJA TOK] Neuspješno ažuriranje palete pri skidanju sa stanja. PaletaId={p.Id}");
                    return new List<Guid>();
                }
            }

            if (preostalo > 0)
                return new List<Guid>();

            return uzeto;
        }

        public bool OdgovaraTipu(Vino v, string naziv, KategorijaVina kategorija, double zapremina)
        {
            if (v == null)
                return false;

            string n1;
            if (v.Naziv == null)
            {
                n1 = string.Empty;
            }
            else
            {
                n1 = v.Naziv.Trim();
            }

            string n2;
            if (naziv == null)
            {
                n2 = string.Empty;
            }
            else
            {
                n2 = naziv.Trim();
            }

            if (!string.Equals(n1, n2, StringComparison.OrdinalIgnoreCase))
                return false;


            if (v.Kategorija != kategorija)
                return false;

            if (Math.Abs(v.ZapreminaLitara - zapremina) > 0.0001)
                return false;

            return true;
        }
    }
}
