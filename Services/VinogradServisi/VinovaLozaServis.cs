using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.VinogradServisi
{
    public class VinovaLozaServis : IVinovaLozaServis
    {
        private readonly IVinovaLozaRepozitorijum _vinovaLozaRepo;
        private readonly Random _random = new Random();

        public VinovaLozaServis(IVinovaLozaRepozitorijum vinovaLozaRepo)
        {
            _vinovaLozaRepo = vinovaLozaRepo;
        }

        public VinovaLoza ZasadiLozu(string naziv)
        {
            double brix = Math.Round(15.0 + _random.NextDouble() * (20.8 - 15.0), 2);
            int godinaSadnje = DateTime.Now.Year;
            string region = "Toskana";

            return ZasadiLozu(naziv, brix, godinaSadnje, region);
        }

        public VinovaLoza ZasadiLozu(string naziv, double nivoSeceraBrix, int godinaSadnje, string region)
        {
            var novaLoza = new VinovaLoza(
                Guid.NewGuid(),
                naziv,
                nivoSeceraBrix,
                godinaSadnje,
                region,
                FazaZrelostiLoze.Posadjena
            );

            return _vinovaLozaRepo.DodajVinovuLozu(novaLoza);
        }
    }
}
