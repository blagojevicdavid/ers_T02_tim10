using System.Collections.Generic;
using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IProizvodnjaVinaServis
    {
        List<Vino> ProizvediVina(
            string nazivVina,
            KategorijaVina kategorija,
            int brojFlasa,
            double zapreminaFlaseLitara);
    }
}
