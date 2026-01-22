using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface IEvidencijaProizvodnjeVinaRepozitorijum
    {
        EvidencijaProizvodnjeVina DodajEvidenciju(EvidencijaProizvodnjeVina evidencija);

        IEnumerable<EvidencijaProizvodnjeVina> SveEvidencije();

        IEnumerable<EvidencijaProizvodnjeVina> EvidencijeZaFermentaciju(Guid fermentacijaId);
    }
}
