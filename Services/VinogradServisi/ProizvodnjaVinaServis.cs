using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.VinoServisi
{
    public class ProizvodnjaVinaServis : IProizvodnjaVinaServis
    {
        private const double LitaraPoLozi = 1.2;
        private const double OptimalniBrix = 24.0;

        private const double KgGrozdjaPoLitru = 1.4;

        private readonly IVinovaLozaRepozitorijum _lozaRepo;
        private readonly IVinoRepozitorijum _vinoRepo;
        private readonly IVinovaLozaServis _vinovaLozaServis;
        private readonly IBerbaLozeServis _berbaServis;
        private readonly IFermentacijaServis _fermentacijaServis;
        private readonly IMerenjeSeceraServis _merenjeSeceraServis;
        private readonly IEvidencijaProizvodnjeVinaServis _evidencijaProizvodnjeServis;
        private readonly ILoggerServis _logger;
        public ProizvodnjaVinaServis(IVinovaLozaRepozitorijum lozaRepo,IVinoRepozitorijum vinoRepo,IVinovaLozaServis vinovaLozaServis,IBerbaLozeServis berbaServis,IFermentacijaServis fermentacijaServis,
            IMerenjeSeceraServis merenjeSeceraServis,IEvidencijaProizvodnjeVinaServis evidencijaProizvodnjeServis,ILoggerServis logger)
        {
            _lozaRepo = lozaRepo;
            _vinoRepo = vinoRepo;
            _vinovaLozaServis = vinovaLozaServis;
            _berbaServis = berbaServis;
            _fermentacijaServis = fermentacijaServis;
            _merenjeSeceraServis = merenjeSeceraServis;
            _evidencijaProizvodnjeServis = evidencijaProizvodnjeServis;
            _logger = logger;
        }

        public List<Vino> ProizvediVina(string nazivVina, KategorijaVina kategorija, int brojFlasa, double zapreminaFlaseLitara)
        {
            if (_lozaRepo == null || _vinoRepo == null || _vinovaLozaServis == null || _berbaServis == null ||
                _fermentacijaServis == null || _merenjeSeceraServis == null || _evidencijaProizvodnjeServis == null || _logger == null)
                return new List<Vino>();

            string naziv = nazivVina == null ? string.Empty : nazivVina.Trim();
            if (naziv.Length == 0) return new List<Vino>();
            if (brojFlasa <= 0) return new List<Vino>();
            if (zapreminaFlaseLitara <= 0) return new List<Vino>();

            double ukupnoLitara = brojFlasa * zapreminaFlaseLitara;
            int potrebneLoze = (int)Math.Ceiling(ukupnoLitara / LitaraPoLozi);

            _logger.Evidentiraj(
                TipEvidencije.INFO,
                $"Proizvodnja: start. Vino={naziv}, Kat={kategorija}, Flase={brojFlasa}, Zap={zapreminaFlaseLitara}, L={ukupnoLitara:0.##}");

            var spremne = (_lozaRepo.PronadjiVinoveLozePoFazi(FazaZrelostiLoze.SpremnaZaBerbu) ?? Enumerable.Empty<VinovaLoza>())
                .ToList();

            while (spremne.Count < potrebneLoze)
            {
                var nova = _vinovaLozaServis.ZasadiLozu(naziv);
                nova.FazaZrelosti = FazaZrelostiLoze.SpremnaZaBerbu;
                _lozaRepo.AzurirajVinovuLozu(nova);
                spremne.Add(nova);

                _logger.Evidentiraj(TipEvidencije.INFO, $"Proizvodnja: zasadjena nova loza ({nova.Id}).");
            }

            var izabrane = spremne.Take(potrebneLoze).ToList();

            foreach (var loza in izabrane)
            {
                if (loza.NivoSeceraBrix > OptimalniBrix)
                {
                    double odstupanje = loza.NivoSeceraBrix - OptimalniBrix;

                    var balans = _vinovaLozaServis.ZasadiLozu(loza.Naziv);
                    balans.FazaZrelosti = FazaZrelostiLoze.SpremnaZaBerbu;

                    balans.NivoSeceraBrix = Math.Round(balans.NivoSeceraBrix - odstupanje, 2);
                    if (balans.NivoSeceraBrix < 0) balans.NivoSeceraBrix = 0;

                    _lozaRepo.AzurirajVinovuLozu(balans);
                    _logger.Evidentiraj(TipEvidencije.INFO, $"Proizvodnja: balans secera, dodata loza ({balans.Id}).");
                }

                loza.FazaZrelosti = FazaZrelostiLoze.Obrana;
                _lozaRepo.AzurirajVinovuLozu(loza);
            }

            double kolicinaKg = Math.Round(ukupnoLitara * KgGrozdjaPoLitru, 2);
            var berba = _berbaServis.EvidentirajBerbu(DateTime.Now, kolicinaKg);
            if (berba == null || berba.Id == Guid.Empty)
            {
                _logger.Evidentiraj(TipEvidencije.ERROR, "Proizvodnja: berba nije uspela.");
                return new List<Vino>();
            }

            _logger.Evidentiraj(TipEvidencije.INFO, $"Proizvodnja: evidentirana berba ({berba.Id}), kg={kolicinaKg:0.##}.");


            var fermentacija = _fermentacijaServis.ZapocniFermentaciju(berba.Id);
            if (fermentacija == null || fermentacija.Id == Guid.Empty)
            {
                _logger.Evidentiraj(TipEvidencije.ERROR, "Proizvodnja: fermentacija nije pokrenuta.");
                return new List<Vino>();
            }

            _logger.Evidentiraj(TipEvidencije.INFO, $"Proizvodnja: fermentacija pokrenuta ({fermentacija.Id}).");
            _fermentacijaServis.PromeniFazu(fermentacija.Id, FazaFermentacije.Aktivna);


            _merenjeSeceraServis.DodajMerenje(fermentacija.Id, 24.0, "Start");
            _merenjeSeceraServis.DodajMerenje(fermentacija.Id, 12.0, "Tok");
            _merenjeSeceraServis.DodajMerenje(fermentacija.Id, 2.0, "Pred kraj");

            _fermentacijaServis.PromeniFazu(fermentacija.Id, FazaFermentacije.Zavrsena);
            _logger.Evidentiraj(TipEvidencije.INFO, $"Proizvodnja: fermentacija zavrsena ({fermentacija.Id}).");


            var evid = _evidencijaProizvodnjeServis.ZabeleziProizvodnju(fermentacija.Id,naziv,brojFlasa,zapreminaFlaseLitara,
                "Pokrenuta proizvodnja (nedovoljno zaliha)");

            if (evid == null || evid.Id == Guid.Empty)
                _logger.Evidentiraj(TipEvidencije.WARNING, "Proizvodnja: evidencija proizvodnje nije sacuvana.");
            else
                _logger.Evidentiraj(TipEvidencije.INFO, $"Proizvodnja: evidencija sacuvana ({evid.Id}).");


            var proizvedena = new List<Vino>(brojFlasa);
            DateTime flasirano = DateTime.UtcNow;

            for (int i = 0; i < brojFlasa; i++)
            {
                Guid id = Guid.NewGuid();
                Guid lozaId = izabrane[i % izabrane.Count].Id;

                var vino = new Vino
                {
                    Id = id,
                    Naziv = naziv,
                    Kategorija = kategorija,
                    ZapreminaLitara = zapreminaFlaseLitara,
                    Sifra = $"VN-2025-{id}",
                    VinovaLozaId = lozaId,
                    DatumFlasiranja = flasirano
                };

                _vinoRepo.DodajVino(vino);
                proizvedena.Add(vino);
            }

            _logger.Evidentiraj(TipEvidencije.INFO, $"Proizvodnja: flasirano {proizvedena.Count} komada ({naziv}).");
            return proizvedena;
        }
    }
}