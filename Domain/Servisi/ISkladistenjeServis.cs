using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;

namespace Domain.Servisi
{
    public interface ISKladistenjeServis
    {
       void PostaviNacinSkladistenja(NacinSkladistenja nacin);
        NacinSkladistenja PreuzmiNacinSkladistenja();
    }
}
