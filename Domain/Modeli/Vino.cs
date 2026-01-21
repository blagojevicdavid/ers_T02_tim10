using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Domain.Modeli
{
    public class Vino
    {
        public Guid Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public KategorijaVina Kategorija { get; set; }
        public double ZapreminaLitara { get; set; }
        public string Sifra { get; set; } = string.Empty;
        public Guid VinovaLozaId { get; set; }
        public DateTime DatumFlasiranja { get; set; }

        public Vino()
        {
            Id = Guid.NewGuid();
        }

        public Vino(Guid id, string naziv, KategorijaVina kategorija, double zapreminaLitara, string sifra, Guid vinovaLozaId, DateTime datumFlasiranja)
        {
            Id = id;
            Naziv = naziv;
            Kategorija = kategorija;
            ZapreminaLitara = zapreminaLitara;
            Sifra = sifra;
            VinovaLozaId = vinovaLozaId;
            DatumFlasiranja = datumFlasiranja;
        }
    }
}
