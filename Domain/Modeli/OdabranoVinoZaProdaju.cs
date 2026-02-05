using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class OdabranoVinoZaProdaju
    {
        public Vino Vino { get; }
        public int Kolicina { get; }

        public OdabranoVinoZaProdaju(Vino vino, int kolicina)
        {
            Vino = vino;
            Kolicina = kolicina;
        }
    }
}
