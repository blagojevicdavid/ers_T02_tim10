using System;
using System.Collections.Generic;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;

namespace Services.SkladistenjeServisi
{
    public class SkladistenjeServis : ISkladistenjeServis
    {
        private NacinSkladistenja izabraniNacin;
        private Guid izabraniVinskiPodrumId;
        private Guid izabraniLokalniPodrumId;

        private bool imaNacin;
        private bool imaVinski;
        private bool imaLokalni;

        public SkladistenjeServis()
        {
            imaNacin = false;
            imaVinski = false;
            imaLokalni = false;
            izabraniVinskiPodrumId = Guid.Empty;
            izabraniLokalniPodrumId = Guid.Empty;
        }

        public void PostaviNacinSkladistenja(NacinSkladistenja nacin)
        {
            izabraniNacin = nacin;
            imaNacin = true;
        }

        public NacinSkladistenja PreuzmiNacinSkladistenja()
        {
            if (!imaNacin) return default(NacinSkladistenja);
            return izabraniNacin;
        }

        public void PostaviVinskiPodrum(Guid vinskiPodrumId)
        {
            if (vinskiPodrumId == Guid.Empty) return;

            izabraniVinskiPodrumId = vinskiPodrumId;
            imaVinski = true;

            imaLokalni = false;
            izabraniLokalniPodrumId = Guid.Empty;
        }

        public Guid PreuzmiVinskiPodrum()
        {
            if (!imaVinski) return Guid.Empty;
            return izabraniVinskiPodrumId;
        }

        public void PostaviLokalniPodrum(Guid lokalniPodrumId)
        {
            if (lokalniPodrumId == Guid.Empty) return;

            izabraniLokalniPodrumId = lokalniPodrumId;
            imaLokalni = true;

            imaVinski = false;
            izabraniVinskiPodrumId = Guid.Empty;
        }

        public Guid PreuzmiLokalniPodrum()
        {
            if (!imaLokalni) return Guid.Empty;
            return izabraniLokalniPodrumId;
        }

        public bool PrihvatiOtpremljenuPaletu(Paleta paleta)
        {

            return paleta != null && paleta.Id != Guid.Empty;

            return true;     //ne treba nista 

        }

        public IEnumerable<Paleta> IsporuciPaleteZaProdaju(int brojPaleta)
        {
            return new List<Paleta>();
        }
    }
}
