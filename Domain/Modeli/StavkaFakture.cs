using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class StavkaFakture
    {
        public Guid VinoId { get; set; }
        public int Kolicina { get; set; }
        public decimal CenaPoKomadu { get; set; }

        public StavkaFakture() { }

        public StavkaFakture(Guid vinoId, int kolicina, decimal cenaPoKomadu)
        {
            VinoId = vinoId;
            Kolicina = kolicina;
            CenaPoKomadu = cenaPoKomadu;
        }
    }
}


