using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi
{
    public interface IIsporukaVinaServis
    {
        List<Paleta> IsporuciPalete(int brojPaleta);
        void PosaljiZahtjev(ZahtjevZaIsporuku zahtjev);

    }
}
