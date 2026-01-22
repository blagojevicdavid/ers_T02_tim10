using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IEvidencijaProizvodnjeVinaServis
    {
        EvidencijaProizvodnjeVina ZabeleziProizvodnju(
        Guid fermentacijaId,
        string nazivVina,
        int brojFlasa,
        double zapreminaFlaseLitara,
        string napomena = ""
       );
     IEnumerable<EvidencijaProizvodnjeVina> PregledSvihEvidencija();

     IEnumerable<EvidencijaProizvodnjeVina> PregledEvidencijaZaFermentaciju(Guid fermentacijaId);

    }
}
