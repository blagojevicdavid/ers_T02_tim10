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

        bool PronadjiVinoPoId(Guid id, out Vino vino); //umjesto bool bilo Vino

        IEnumerable<Vino> PronadjiVinaPoKategoriji(KategorijaVina kategorija);
    }
}
