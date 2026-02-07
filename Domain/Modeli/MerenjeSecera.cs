using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class MerenjeSecera
    {
        public Guid Id { get; set; }
        public Guid FermentacijaId { get; set; }
        public double NivoSeceraBrix { get; set; }
        public DateTime DatumVreme { get; set; }
        public string Napomena { get; set; } = string.Empty;

        public MerenjeSecera()
        {
        }
        public MerenjeSecera(Guid fermentacijaId, double nivoSeceraBrix,DateTime datumVreme,string napomena)
        {
            Id = Guid.NewGuid();
            FermentacijaId = fermentacijaId;
            NivoSeceraBrix = nivoSeceraBrix;
            DatumVreme = datumVreme;
            Napomena = napomena;
        }
    }
}
