using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Servisi;
using Domain.Modeli;

namespace Services.VinoServisi
{
    public class OdabirKolicineVinaServis : IOdabirKolicineVinaServis
    {
        private readonly IPonudaVinaServis ponudaVinaServis;

        public OdabirKolicineVinaServis(IPonudaVinaServis ponudaVinaServis)
        {
            this.ponudaVinaServis = ponudaVinaServis;
        }

        public OdabranoVinoZaProdaju? Odaberi(string sifra, int kolicina)
        {
            if (string.IsNullOrWhiteSpace(sifra)) return null;
            if (kolicina <= 0) return null;

            var vino = ponudaVinaServis.PronadjiPoSifri(sifra.Trim());
            if (vino == null) return null;

            return new OdabranoVinoZaProdaju(vino, kolicina);
        }
    }
}
