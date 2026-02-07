using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class LogZapis
    {
        public TipEvidencije Tip { get; }
        public DateTime DatumVreme { get; }
        public string Poruka { get; }

        public LogZapis(TipEvidencije tip, string poruka)
        {
            Tip = tip;
            Poruka = poruka;
            DatumVreme = DateTime.Now;
        }

        public override string ToString()
        {
            return $"[{DatumVreme:yyyy-MM-dd HH:mm:ss}] {Tip}: {Poruka}";
        }
    }
}
