using Domain.Enumeracije;
using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Repozitorijumi
{
    public interface IVinovaLozaRepozitorijum
    {
        bool AzurirajVinovuLozu(VinovaLoza loza);

        VinovaLoza DodajVinovuLozu(VinovaLoza loza);

        VinovaLoza PronadjiVinovuLozuPoId(Guid id);

        IEnumerable<VinovaLoza> PronadjiVinoveLozePoFazi(FazaZrelostiLoze faza);
    }
}

