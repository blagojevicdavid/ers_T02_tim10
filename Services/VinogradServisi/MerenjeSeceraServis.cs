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
            var fermentacija = fermentacijaRepo.PronadjiFermentacijuPoId(fermentacijaId);
            if(fermentacija == null) throw new Exception("Fermentacija nije pronadjena.");

            var merenje = new MerenjeSecera
            {
                Id = Guid.NewGuid(),
                FermentacijaId = fermentacijaId,
                NivoSeceraBrix = nivoSeceraBrix,
                DatumVreme = DateTime.UtcNow,
                Napomena = napomena ?? ""
            };

            merenjeRepo.DodajMerenje(merenje);

            //azuriram nivo i u fermentaciji kad sam vec tu
            fermentacija.PoslednjiBrix = nivoSeceraBrix;
            fermentacijaRepo.AzurirajFermentaciju(fermentacija);  //povratna vr provjera

            return merenje;
        }
        public IEnumerable<MerenjeSecera> PregledMerenja(Guid farmentacijaId)
        {
            return merenjeRepo.MerenjaZaFermentaciju(farmentacijaId);
        }
    }
}
