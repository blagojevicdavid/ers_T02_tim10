using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface IMerenjeSeceraRepozitorijum
    {
        MerenjeSecera DodajMerenje(MerenjeSecera merenje);

        IEnumerable<MerenjeSecera> SvaMerenja();

        IEnumerable<MerenjeSecera> MerenjaZaFermentaciju(Guid fermentacijaId);
    }
}
