using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repozitorijumi
{
    public interface IBerbaLozeRepozitorijum
    {
        BerbaLoze Dodaj(BerbaLoze berba);
        IEnumerable<BerbaLoze> Sve();
        BerbaLoze PronadjiPoId(Guid id);
        bool Azuriraj(BerbaLoze berba);
    }
}
