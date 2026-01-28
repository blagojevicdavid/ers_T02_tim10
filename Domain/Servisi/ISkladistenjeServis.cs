using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD

using Domain.Modeli;
using System.Collections.Generic;

namespace Domain.Servisi
{
    public interface ISkladistenjeServis
    {
        bool PrihvatiOtpremljenuPaletu(Paleta paleta);
        IEnumerable<Paleta> IsporuciPaleteZaProdaju(int brojPaleta);
=======
using Domain.Enumeracije;

namespace Domain.Servisi
{
    public interface ISKladistenjeServis
    {
       void PostaviNacinSkladistenja(NacinSkladistenja nacin);
        NacinSkladistenja PreuzmiNacinSkladistenja();

        void PostaviVinskiPodrum(Guid vinskiPodrumId);
        Guid PreuzmiVinskiPodrum();

        void PostaviLokalniPodrum(Guid lokalniPodrumId);
        Guid PreuzmiLokalniPodrum();
>>>>>>> 7d356f703eacf9bf164455f1f8b479e0d8615f72
    }
}
