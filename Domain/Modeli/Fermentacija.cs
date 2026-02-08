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
        public Guid Id { get; set; } = Guid.Empty;
        public Guid BerbaId { get; set; } = Guid.Empty;
        public Guid VinoId { get; set; } = Guid.Empty;
        public DateTime DatumPocetka { get; set; } = DateTime.MinValue;
        public DateTime DatumZavrsetka { get; set; } = DateTime.MinValue;
        public FazaFermentacije Faza { get; set; }
        public double PoslednjiBrix { get; set; } = 0;
        public double PoslednjaTemperaturaC { get; set; } = 0;
        public string Napomena { get; set; } = string.Empty;


        public Fermentacija()
        {
        }

        public Fermentacija(Guid berbaId, DateTime datumPocetka,FazaFermentacije faza,Guid vinoId,DateTime datumZavrsetka,double poslednjiBrix,double poslednjaTemperaturaC,string napomena)
        {
            Id = Guid.NewGuid();
            BerbaId = berbaId;
            DatumPocetka = datumPocetka;
            Faza = faza;
            VinoId = vinoId;
            DatumZavrsetka = datumZavrsetka;
            PoslednjiBrix = poslednjiBrix;
            PoslednjaTemperaturaC = poslednjaTemperaturaC;
            Napomena = napomena;

        }


    }
}
