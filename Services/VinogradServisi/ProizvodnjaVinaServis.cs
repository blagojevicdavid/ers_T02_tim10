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

        private readonly IVinovaLozaRepozitorijum _lozaRepo;
        private readonly IVinoRepozitorijum _vinoRepo;
        private readonly IVinovaLozaServis _vinovaLozaServis;
        private readonly ILoggerServis _logger;

        public ProizvodnjaVinaServis(
            IVinovaLozaRepozitorijum lozaRepo,
            IVinoRepozitorijum vinoRepo,
            IVinovaLozaServis vinovaLozaServis,
            ILoggerServis logger)
        {
            _lozaRepo = lozaRepo;
            _vinoRepo = vinoRepo;
            _vinovaLozaServis = vinovaLozaServis;
            _logger = logger;
        }

        public List<Vino> ProizvediVina(string nazivVina, KategorijaVina kategorija, int brojFlasa, double zapreminaFlaseLitara)
        {
            double ukupnoLitara = brojFlasa * zapreminaFlaseLitara;
            int potrebneLoze = (int)Math.Ceiling(ukupnoLitara / LitaraPoLozi);

            var spremne = (_lozaRepo.PronadjiVinoveLozePoFazi(FazaZrelostiLoze.SpremnaZaBerbu) ?? Enumerable.Empty<VinovaLoza>())
                .ToList();

            while (spremne.Count < potrebneLoze)
            {
                var nova = _vinovaLozaServis.ZasadiLozu(nazivVina.Trim());
                nova.FazaZrelosti = FazaZrelostiLoze.SpremnaZaBerbu;
                _lozaRepo.AzurirajVinovuLozu(nova);
                spremne.Add(nova);
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
                }

                loza.FazaZrelosti = FazaZrelostiLoze.Obrana;
                _lozaRepo.AzurirajVinovuLozu(loza);
            }

            var proizvedena = new List<Vino>(brojFlasa);
            DateTime flasirano = DateTime.UtcNow;

            for (int i = 0; i < brojFlasa; i++)
            {
                Guid id = Guid.NewGuid();
                Guid lozaId = izabrane[i % izabrane.Count].Id;

                var vino = new Vino
                {
                    Id = id,
                    Naziv = nazivVina.Trim(),
                    Kategorija = kategorija,
                    ZapreminaLitara = zapreminaFlaseLitara,
                    Sifra = $"VN-2025-{id}",
                    VinovaLozaId = lozaId,
                    DatumFlasiranja = flasirano
                };

                _vinoRepo.DodajVino(vino);
                proizvedena.Add(vino);
            }

            return proizvedena;
        }
    }
}
