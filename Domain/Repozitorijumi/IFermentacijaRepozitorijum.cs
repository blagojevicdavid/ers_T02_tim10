using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Modeli;
using Domain.Enumeracije;

namespace Domain.Repozitorijumi
{
    public interface IFermentacijaRepozitorijum
    {
        bool AzurirajFermentaciju(Fermentacija fermentacija);

        Fermentacija DodajFermentaciju(Fermentacija fermentacija);

        Fermentacija PronadjiFermentacijuPoId(Guid id);

        IEnumerable<Fermentacija> SveFermentacije();

    }
}
