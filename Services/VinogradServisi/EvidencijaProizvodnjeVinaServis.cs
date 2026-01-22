using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // Provera fermentacije
            var fermentacija = fermentacijaRepo.PronadjiFermentacijuPoId(fermentacijaId);
            if (fermentacija == null)
                throw new Exception("Fermentacija nije pronađena.");

            // racunanje ukupne količine
            double ukupnoLitara = brojFlasa * zapreminaFlaseLitara;

            var evidencija = new EvidencijaProizvodnjeVina
            {
                Id = Guid.NewGuid(),
                FermentacijaId = fermentacijaId,
                NazivVina = nazivVina ?? "",
                BrojFlasa = brojFlasa,
                ZapreminaFlaseLitara = zapreminaFlaseLitara,
                UkupnoLitara = ukupnoLitara,
                DatumVreme = DateTime.UtcNow,
                napomena = napomena ?? ""
            };

            return evidencijaRepo.DodajEvidenciju(evidencija);
        }

        public IEnumerable<EvidencijaProizvodnjeVina> PregledSvihEvidencija()
        {
            return evidencijaRepo.SveEvidencije();
        }

        public IEnumerable<EvidencijaProizvodnjeVina> PregledEvidencijaZaFermentaciju(Guid fermentacijaId)
        {
            return evidencijaRepo.EvidencijeZaFermentaciju(fermentacijaId);
        }
    }
}
