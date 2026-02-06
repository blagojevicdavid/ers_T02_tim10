using System;
using System.Collections.Generic;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;

namespace Services.SkladistenjeServisi
{
    public class SkladistenjeServis : ISkladistenjeServis
    {
        private NacinSkladistenja? izabraniNacin;

        private Guid? izabraniVinskiPodrumId;
        private Guid? izabraniLokalniPodrumId;

        public void PostaviNacinSkladistenja(NacinSkladistenja nacin)
        {
            izabraniNacin = nacin;
        }

        public NacinSkladistenja PreuzmiNacinSkladistenja()
        {
            if (izabraniNacin == null)
                throw new InvalidOperationException("Način skladištenja nije izabran.");
            return izabraniNacin.Value;
        }

        public void PostaviVinskiPodrum(Guid vinskiPodrumId)
        {
            if (vinskiPodrumId == Guid.Empty)
                throw new ArgumentException("Neispravan ID vinskog podruma.");

            izabraniVinskiPodrumId = vinskiPodrumId;
            izabraniLokalniPodrumId = null;
        }

        public Guid PreuzmiVinskiPodrum()
        {
            if (izabraniVinskiPodrumId == null)
                throw new InvalidOperationException("Vinski podrum nije izabran.");
            return izabraniVinskiPodrumId.Value;
        }

        public void PostaviLokalniPodrum(Guid lokalniPodrumId)
        {
            if (lokalniPodrumId == Guid.Empty)
                throw new ArgumentException("Neispravan ID lokalnog podruma.");

            izabraniLokalniPodrumId = lokalniPodrumId;
            izabraniVinskiPodrumId = null;
        }

        public Guid PreuzmiLokalniPodrum()
        {
            if (izabraniLokalniPodrumId == null)
                throw new InvalidOperationException("Lokalni podrum nije izabran.");
            return izabraniLokalniPodrumId.Value;
        }

        public bool PrihvatiOtpremljenuPaletu(Paleta paleta)
        {
            return true;
        }

        public IEnumerable<Paleta> IsporuciPaleteZaProdaju(int brojPaleta)
        {
            return new List<Paleta>();
        }
    }
}
