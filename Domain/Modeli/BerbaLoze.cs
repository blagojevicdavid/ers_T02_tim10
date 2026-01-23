using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class BerbaLoze
    {
        public Guid Id { get; set;  }
        public DateTime DatumBerbe { get; set; }
        public double KolicinaKg { get; set; }

        public BerbaLoze() { }

        public BerbaLoze(Guid id, DateTime datumBerbe, double kolicinaKg)
        {
            Id = id;
            DatumBerbe = datumBerbe;
            KolicinaKg = kolicinaKg;
        }
    }
}
