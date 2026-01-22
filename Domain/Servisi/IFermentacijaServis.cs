using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IFermentacijaServis
    {
        Fermentacija ZapocniFermentaciju(Guid lozaId);

        bool PromeniFazu(Guid fermentacijaId, FazaFermentacije novaFaza);

        IEnumerable<Fermentacija> PregledSvihFermentacija();

        Fermentacija PregledFermentacije(Guid fermentacijaId);


    }
}
