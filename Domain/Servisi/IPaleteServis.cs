using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Servisi
{
    public interface IPaleteServis
    {
        IList<Paleta> PosaljiPaleteUVinskiPodrum(Guid vinskiPodrumId, int brojPaleta);

        Paleta KreirajNovuPaletu(string adresaOdredista);

        bool DodajProizvedenoVinoNaPaletu(Guid paletaId, Guid evidencijaProizvodnjevinaId);

        Paleta PregledPalete(Guid paletaId);
    }
}
