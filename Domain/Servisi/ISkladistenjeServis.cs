using System;
using System.Collections.Generic;
using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface ISkladistenjeServis
    {
        // izbor načina skladištenja
        void PostaviNacinSkladistenja(NacinSkladistenja nacin);
        NacinSkladistenja PreuzmiNacinSkladistenja();

        // izbor podruma
        void PostaviVinskiPodrum(Guid vinskiPodrumId);
        Guid PreuzmiVinskiPodrum();

        void PostaviLokalniPodrum(Guid lokalniPodrumId);
        Guid PreuzmiLokalniPodrum();

        // logika skladištenja paleta (PakovanjeServis ovo koristi)
        bool PrihvatiOtpremljenuPaletu(Paleta paleta);
        IEnumerable<Paleta> IsporuciPaleteZaProdaju(int brojPaleta);
    }
}
