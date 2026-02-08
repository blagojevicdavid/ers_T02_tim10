using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi
{
    public interface IVinovaLozaServis
    {
        VinovaLoza ZasadiLozu(string naziv);
        VinovaLoza ZasadiLozu(string naziv,double nivoSeceraBrix,int godinaSadnje,string region);
    }
}
