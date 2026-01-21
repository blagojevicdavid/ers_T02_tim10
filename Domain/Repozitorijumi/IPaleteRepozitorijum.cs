using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Modeli;
using Domain.Enumeracije;

namespace Domain.Repozitorijumi
{
    public interface IPaleteRepozitorijum
    {
        bool AzurirajPaletu(Paleta paleta);

        Paleta DodajPaletu(Paleta paleta);

        Paleta PronadjiPaletuPoId(Guid id);

        IEnumerable<Paleta> PronadjiPaletePoStatusu(StatusPalete status);
    }
}

