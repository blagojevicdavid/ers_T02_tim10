using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class BerbaLoze
    {
        public Guid Id { get; }
        public DateTime DatumBerbe { get; }
        public double KolicinaKg { get; }

        public BerbaLoze(Guid id, DateTime datumBerbe, double kolicinaKg)
        {
            Id = id;
            DatumBerbe = datumBerbe;
            KolicinaKg = kolicinaKg;
        }
    }
}
