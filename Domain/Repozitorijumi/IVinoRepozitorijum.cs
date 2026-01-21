using Domain.Enumeracije;
using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Repozitorijumi
{
    public interface IVinoRepozitorijum
    {
        bool AzurirajVino(Vino vino);

        Vino DodajVino(Vino vino);

        Vino PronadjiVinoPoId(Guid id);

        IEnumerable<Vino> PronadjiVinaPoKategoriji(KategorijaVina kategorija);
    }
}
