using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi
{
    public interface IProdajaServis
    {
        Guid IsporuciVinoKupcu(Guid paletaId,string kupac,decimal cenaPoKomadu,TipProdaje tipProdaje,NacinPlacanja nacinPlacanja);
    }
}
