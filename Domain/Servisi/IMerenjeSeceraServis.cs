using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IMerenjeSeceraServis
    {
        MerenjeSecera DodajMerenje(
            Guid fermentacijaId,
            double nivoSeceraBrix,
            string napmena = "");

        IEnumerable<MerenjeSecera> PregledMerenja(Guid fermentacijaId);


    }

}
