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
        public string NazivVina { get; set; } = string.Empty;
        public int BrojFlasa { get; set; }
        public double ZapreminaFlaseLitara { get; set; }
        public double UkupnoLitara { get; set; }
        public DateTime DatumVreme { get; set; }
        public string napomena { get; set; } = string.Empty;

        public EvidencijaProizvodnjeVina()
        {
        }

        public EvidencijaProizvodnjeVina(Guid fermentacijaId, string nazivVina, int brojFlasa, double zapreminaFlaseLitara,DateTime datumVreme, string Napomena)
        {
            FermentacijaId = fermentacijaId;
            NazivVina = nazivVina;
            BrojFlasa = brojFlasa;
            ZapreminaFlaseLitara = zapreminaFlaseLitara;
            UkupnoLitara = brojFlasa * zapreminaFlaseLitara;
            DatumVreme = datumVreme;
            napomena = Napomena;
        }
    }
    }
 