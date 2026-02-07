using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;


namespace Domain.Modeli
{
    public class Faktura
    {
        public Guid Id { get; set; }
        public DateTime DatumIzdavanja { get; set; }
        public TipProdaje TipProdaje { get; set; }
        public NacinPlacanja NacinPlacanja { get; set; }
        public List<StavkaFakture> Stavke { get; set; } = new();
        
        public Faktura()
        {
            Id = Guid.NewGuid();
            DatumIzdavanja = DateTime.Now;
        }
        public Faktura(Guid id, DateTime datumIzdavanja, TipProdaje tipProdaje, NacinPlacanja nacinPlacanja, List<StavkaFakture> stavke)
        {
            Id = id;
            DatumIzdavanja = datumIzdavanja;
            TipProdaje = tipProdaje;
            NacinPlacanja = nacinPlacanja;
            Stavke = stavke;
        }
    }
}
