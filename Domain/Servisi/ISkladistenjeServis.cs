using System;
using System.Collections.Generic;
using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface ISkladistenjeServis
    {
        void PostaviNacinSkladistenja(NacinSkladistenja nacin);
        NacinSkladistenja PreuzmiNacinSkladistenja();

        void PostaviVinskiPodrum(Guid vinskiPodrumId);
        Guid PreuzmiVinskiPodrum();

        void PostaviLokalniPodrum(Guid lokalniPodrumId);
        Guid PreuzmiLokalniPodrum();
 
        bool PrihvatiOtpremljenuPaletu(Paleta paleta);
        IEnumerable<Paleta> IsporuciPaleteZaProdaju(int brojPaleta);
    }
}
