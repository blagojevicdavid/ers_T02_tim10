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
    }
}
