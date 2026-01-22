using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.VinogradServisi
{
    public class ProracunGrozdjaServis : IProracunGrozdjaServis
    {
        private const double LITARA_PO_LOZI = 1.2;

        public int IzracunajPotrebnuKolicinuLoza(int brojFlasa, double zapreminaFlase)
        {
            if (brojFlasa <= 0) throw new ArgumentException("Broj flaša mora biti veći od 0.");
            if (zapreminaFlase <= 0) throw new ArgumentException("Zapremina flaše mora biti veća od 0.");

            double ukupnoLitara = brojFlasa * zapreminaFlase;
            return (int)Math.Ceiling(ukupnoLitara / LITARA_PO_LOZI);
        }
    }
}
