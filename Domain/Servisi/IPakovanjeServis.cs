using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Enumeracije;
using Domain.Modeli;


namespace Domain.Servisi
{
    public interface IPakovanjeServis
    {
        (bool, Paleta) UpakujVinaUPaletu(
            string nazivVina,
            KategorijaVina kategorija,
            int brojFlasa,
            double zapreminaFlase,
            string adresaOdredista,
            Guid vinskiPodrumId
        );

        (bool, Paleta) PosaljiPrvuDostupnuUpakovanuPaletu(
            string nazivVina,
            KategorijaVina kategorija,
            int brojFlasa,
            double zapreminaFlase,
            string adresaOdredista,
            Guid vinskiPodrumId
        );
    }
}
