using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.VinogradServisi
{
    public class FermentacijaServis : IFermentacijaServis
    {
        private readonly IFermentacijaRepozitorijum fermentacijaRepo;

        public FermentacijaServis(IFermentacijaRepozitorijum repo)
        {
            fermentacijaRepo = repo;
        }

        public Fermentacija ZapocniFermentaciju(Guid lozaId)
        {
            var f = new Fermentacija
            {
                Id = Guid.NewGuid(),
                LozaId = lozaId,
                DatumPocetka = DateTime.UtcNow,
                Faza = FazaFermentacije.Pokrenuta
            };
            return fermentacijaRepo.DodajFermentaciju(f);
        }
        public bool PromeniFazu(Guid fermentacijaId, FazaFermentacije novaFaza)
        {
            var f = fermentacijaRepo.PronadjiFermentacijuPoId(fermentacijaId);


            if (f == null) return false;

            f.Faza = novaFaza;

            if (novaFaza == FazaFermentacije.Zavrsena)
            {
                f.DatumZavrsetka = DateTime.UtcNow;
            }
            return fermentacijaRepo.AzurirajFermentaciju(f);
        }
        public IEnumerable<Fermentacija> PregledSvihFermentacija()
        {
            return fermentacijaRepo.SveFermentacije();
        }
        public Fermentacija PregledFermentacije(Guid fermentacijaId)
        {
            return fermentacijaRepo.PronadjiFermentacijuPoId(fermentacijaId);
        }
    }
}
