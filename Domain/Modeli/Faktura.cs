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


        public decimal UkupanIznos   //ovo ne bi trebalo da se nalazi ovde? pomocna metoda?
        {
            get
            {
                decimal suma = 0;
                foreach (var s in Stavke)
                    suma += s.CenaPoKomadu * s.Kolicina;
                return suma;
            }
        }


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
            Stavke = stavke ?? new List<StavkaFakture>();
        }
    }
}
