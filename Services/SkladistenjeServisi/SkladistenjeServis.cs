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
                throw new InvalidOperationException("Nacin skladistenja nije izabran.");
            return izabraniNacin.Value;
        }
    }
}
