using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.Servisi;

namespace Services.SkladistenjeServisi
{
    public class SkladistenjeServis : ISKladistenjeServis
    {
        private NacinSkladistenja? izabraniNacin;
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

    private Guid? izabraniVinskiPodrumId;
        public void PostaviVinskiPodrum(Guid vinskiPodrumId)
        {
            if (vinskiPodrumId == Guid.Empty)
                throw new ArgumentException("Neispravan ID vinskog podruma.");
            izabraniVinskiPodrumId = vinskiPodrumId;
        }
        public Guid PreuzmiVinskiPodrum()
        {
            if (izabraniVinskiPodrumId == null)
                throw new InvalidOperationException("Vinski podrum nije izabran.");
            return izabraniVinskiPodrumId.Value;
        }

    private Guid? izabraniLokalniPodrumId;
        public void PostaviLokalniPodrum(Guid lokalniPodrumId)
        {
            if (lokalniPodrumId == Guid.Empty)
                throw new ArgumentException("Neispravan ID lokalnog podruma.");
            izabraniLokalniPodrumId = lokalniPodrumId;
        }
        public Guid PreuzmiLokalniPodrum()
        {
            if (izabraniLokalniPodrumId == null)
                throw new InvalidOperationException("Lokalni podrum nije izabran.");
            return izabraniLokalniPodrumId.Value;
        }
    }
}
