using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;
using Domain.BazaPodataka;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class MerenjeSeceraRepozitorijum : IMerenjeSeceraRepozitorijum
    {
        private readonly IBazaPodataka bazaPodataka;

        public MerenjeSeceraRepozitorijum(IBazaPodataka bazaPodataka)
        {
            this.bazaPodataka = bazaPodataka;
        }
        public MerenjeSecera DodajMerenje(MerenjeSecera merenje)
        {
            bazaPodataka.Tabele.MerenjaSecera.Add(merenje);
            bazaPodataka.SacuvajPromene();
            return merenje;
        }

        public IEnumerable<MerenjeSecera> SvaMerenja()
        {
            return bazaPodataka.Tabele.MerenjaSecera;
        }

        public IEnumerable<MerenjeSecera> MerenjaZaFermentaciju(Guid fermentacija)
        { 
            return bazaPodataka.Tabele.MerenjaSecera.Where(x => x.FermentacijaId == fermentacija).OrderBy(x => x.DatumVreme);
        }
    }
}
