using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Konstante;
namespace Services.VinogradServisi
{
    public class ProracunGrozdjaServis : IProracunGrozdjaServis
    {
        public int IzracunajPotrebnuKolicinuLoza(int brojFlasa, double zapreminaFlase)
        {

            double ukupnoLitara = brojFlasa * zapreminaFlase;
            return (int)Math.Ceiling(ukupnoLitara / ProracunGrozdjaKonstante.LITARA_PO_LOZI);
        }
    }
}
