using System;
using System.Collections.Generic;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.VinogradServisi
{
    public class EvidencijaProizvodnjeVinaServis : IEvidencijaProizvodnjeVinaServis
    {
        private readonly IEvidencijaProizvodnjeVinaRepozitorijum evidencijaRepo;
        private readonly IFermentacijaRepozitorijum fermentacijaRepo;

        public EvidencijaProizvodnjeVinaServis(
            IEvidencijaProizvodnjeVinaRepozitorijum evidencijaRepo,
            IFermentacijaRepozitorijum fermentacijaRepo)
        {
            this.evidencijaRepo = evidencijaRepo;
            this.fermentacijaRepo = fermentacijaRepo;
        }

        public EvidencijaProizvodnjeVina ZabeleziProizvodnju(
            Guid fermentacijaId,
            string nazivVina,
            int brojFlasa,
            double zapreminaFlaseLitara,
            string napomena = "")
        {
            if (evidencijaRepo == null || fermentacijaRepo == null)
                return new EvidencijaProizvodnjeVina();

            if (fermentacijaId == Guid.Empty)
                return new EvidencijaProizvodnjeVina();

            if (brojFlasa <= 0)
                return new EvidencijaProizvodnjeVina();

            if (zapreminaFlaseLitara <= 0)
                return new EvidencijaProizvodnjeVina();

            string naziv = nazivVina == null ? string.Empty : nazivVina.Trim();
            if (naziv.Length == 0)
                return new EvidencijaProizvodnjeVina();

            var fermentacija = fermentacijaRepo.PronadjiFermentacijuPoId(fermentacijaId);
            if (fermentacija == null)
                return new EvidencijaProizvodnjeVina();

            double ukupnoLitara = brojFlasa * zapreminaFlaseLitara;

            var evidencija = new EvidencijaProizvodnjeVina
            {
                Id = Guid.NewGuid(),
                FermentacijaId = fermentacijaId,
                NazivVina = naziv,
                BrojFlasa = brojFlasa,
                ZapreminaFlaseLitara = zapreminaFlaseLitara,
                UkupnoLitara = ukupnoLitara,
                DatumVreme = DateTime.UtcNow,
                napomena = napomena == null ? string.Empty : napomena
            };

            var sacuvana = evidencijaRepo.DodajEvidenciju(evidencija);
            if (sacuvana == null || sacuvana.Id == Guid.Empty)
                return new EvidencijaProizvodnjeVina();

            return sacuvana;
        }

        public IEnumerable<EvidencijaProizvodnjeVina> PregledSvihEvidencija()
        {
            if (evidencijaRepo == null)
                return new List<EvidencijaProizvodnjeVina>();

            var sve = evidencijaRepo.SveEvidencije();
            if (sve == null)
                return new List<EvidencijaProizvodnjeVina>();

            return sve;
        }

        public IEnumerable<EvidencijaProizvodnjeVina> PregledEvidencijaZaFermentaciju(Guid fermentacijaId)
        {
            if (evidencijaRepo == null)
                return new List<EvidencijaProizvodnjeVina>();

            if (fermentacijaId == Guid.Empty)
                return new List<EvidencijaProizvodnjeVina>();

            var lista = evidencijaRepo.EvidencijeZaFermentaciju(fermentacijaId);
            if (lista == null)
                return new List<EvidencijaProizvodnjeVina>();

            return lista;
        }
    }
}
