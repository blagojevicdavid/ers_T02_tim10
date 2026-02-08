using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi;

public interface IProdajaTokServis
{
    Guid IzvrsiProdaju(string nazivVina,KategorijaVina kategorija, int brojFlasa,double zapremina,TipProdaje tipProdaje,NacinPlacanja nacinPlacanja,
                      string adresaOdredista,Guid vinskiPodrumId, string kupac);
}
