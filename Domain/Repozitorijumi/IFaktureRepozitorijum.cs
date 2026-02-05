using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Modeli;
using Domain.Enumeracije;

namespace Domain.Repozitorijumi
{
    public interface IFaktureRepozitorijum
    {
        bool AzurirajFakturu(Faktura faktura);

        Faktura DodajFakturu(Faktura faktura);

        Faktura PronadjiFakturuPoId(Guid id);

        IEnumerable<Faktura> PronadjiFakturePoTipuProdaje(TipProdaje tipProdaje);

        IEnumerable<Faktura> PreuzmiSveFakture();
    }
}

