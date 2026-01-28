using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;

namespace Domain.Servisi
{
    public interface ISKladistenjeServis
    {
       void PostaviNacinSkladistenja(NacinSkladistenja nacin);
        NacinSkladistenja PreuzmiNacinSkladistenja();

        void PostaviVinskiPodrum(Guid vinskiPodrumId);
        Guid PreuzmiVinskiPodrum();

        void PostaviLokalniPodrum(Guid lokalniPodrumId);
        Guid PreuzmiLokalniPodrum();
    }
}
