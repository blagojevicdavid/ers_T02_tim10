using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IsporukaServis
{
    public class IsporukaVinaServis: IIsporukaVinaServis
    {
        private readonly ISkladistenjeServis _skladistenje;
        private readonly ILoggerServis _logger;

        public IsporukaVinaServis(ISkladistenjeServis skladistenje, ILoggerServis logger)
        {
            _skladistenje = skladistenje;
        }

        public void PosaljiZahtjev(ZahtjevZaIsporuku zahtjev)
        {
            

            zahtjev.Status = StatusZahtjeva.UObradi;

            var palete = IsporuciPalete(zahtjev.BrojPaleta);

            if (palete == null || palete.Count == 0)
            {
                zahtjev.Status = StatusZahtjeva.Odbijen;
                
                return;
            }

            zahtjev.Status = StatusZahtjeva.Isporucen;
            
        }

        public List<Paleta> IsporuciPalete(int brojPaleta)
        {
            return _skladistenje.IsporuciPaleteZaProdaju(brojPaleta).ToList();
        }

    }
}
