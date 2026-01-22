using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class EvidencijaProizvodnjeVina
    {
        public Guid Id { get; set; }

        public Guid FermentacijaId { get; set; }

        public string NazivVina { get; set; }

        public int BrojFlasa { get; set; }

        public double ZapreminaFlaseLitara { get; set; }

        public double UkupnoLitara { get; set; }

        public DateTime DatumVreme { get; set; }

        public string napomena { get; set; } = string.Empty;
    }
}
