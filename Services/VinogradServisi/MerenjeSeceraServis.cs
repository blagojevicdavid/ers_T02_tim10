using System;
using System.Collections.Generic;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.VinogradServisi
{
    public class MerenjeSeceraServis : IMerenjeSeceraServis
    {
        private readonly IMerenjeSeceraRepozitorijum merenjeRepo;
        private readonly IFermentacijaRepozitorijum fermentacijaRepo;

        public MerenjeSeceraServis(
            IMerenjeSeceraRepozitorijum merenjeRepo,
            IFermentacijaRepozitorijum fermentacijaRepo)
        {
            this.merenjeRepo = merenjeRepo;
            this.fermentacijaRepo = fermentacijaRepo;
        }

        public MerenjeSecera DodajMerenje(Guid fermentacijaId, double nivoSeceraBrix, string napomena = "")
        {
            if (merenjeRepo == null || fermentacijaRepo == null) return new MerenjeSecera();
            if (fermentacijaId == Guid.Empty) return new MerenjeSecera();

            var fermentacija = fermentacijaRepo.PronadjiFermentacijuPoId(fermentacijaId);
            if (fermentacija == null) return new MerenjeSecera();
            if (fermentacija.Id == Guid.Empty) return new MerenjeSecera();

            var merenje = new MerenjeSecera
            {
                Id = Guid.NewGuid(),
                FermentacijaId = fermentacijaId,
                NivoSeceraBrix = nivoSeceraBrix,
                DatumVreme = DateTime.UtcNow,
                Napomena = napomena == null ? string.Empty : napomena
            };

            merenjeRepo.DodajMerenje(merenje);

            fermentacija.PoslednjiBrix = nivoSeceraBrix;
            fermentacijaRepo.AzurirajFermentaciju(fermentacija);

            return merenje;
        }

        public IEnumerable<MerenjeSecera> PregledMerenja(Guid farmentacijaId)
        {
            if (merenjeRepo == null) return new List<MerenjeSecera>();
            if (farmentacijaId == Guid.Empty) return new List<MerenjeSecera>();

            var lista = merenjeRepo.MerenjaZaFermentaciju(farmentacijaId);
            if (lista == null) return new List<MerenjeSecera>();

            return lista;
        }
    }
}
