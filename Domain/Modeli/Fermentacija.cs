using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class Fermentacija
    {
        public Guid Id { get; set; }

        public Guid LozaId { get; set; }

        public Guid? VinoId { get; set; }

        public DateTime DatumPocetka { get; set; }
        public DateTime? DatumZavrsetka { get; set; }

        public FazaFermentacije Faza { get; set; }

        public double? PoslednjiBrix { get; set; }
        public double? PoslednjaTemperaturaC { get; set; }

        public string Napomena { get; set; } = string.Empty;
    }
}
