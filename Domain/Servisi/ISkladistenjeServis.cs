using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Modeli;
using System.Collections.Generic;

namespace Domain.Servisi
{
    public interface ISkladistenjeServis
    {
        bool PrihvatiOtpremljenuPaletu(Paleta paleta);
        IEnumerable<Paleta> IsporuciPaleteZaProdaju(int brojPaleta);
    }
}
