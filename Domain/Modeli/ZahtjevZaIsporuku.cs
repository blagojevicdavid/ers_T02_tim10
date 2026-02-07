using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class ZahtjevZaIsporuku
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int BrojPaleta { get; set; }
        public DateTime VrijemeZahtjeva { get; set; } = DateTime.Now;
        public StatusZahtjeva Status { get; set; } = StatusZahtjeva.Kreiran;

        public ZahtjevZaIsporuku()
        {
        }

       public ZahtjevZaIsporuku(Guid id, int brojPaleta, DateTime vrijemeZahtjeva,StatusZahtjeva status)
        {
            Id = id;
            BrojPaleta = brojPaleta;
            VrijemeZahtjeva = vrijemeZahtjeva;
            Status = status;
        }
    }

}
