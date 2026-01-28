using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class VinskiPodrum
    {
        public Guid Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public double TemperaturaSkladistenja { get; set; }
        public int MaksimalanBrojPaleta { get; set; }

        // XML-friendly
        public VinskiPodrum()
        {
            Id = Guid.NewGuid();
        }

        public VinskiPodrum(Guid id, string naziv, double temperaturaSkladistenja, int maksimalanBrojPaleta)
        {
            Id = id;
            Naziv = naziv;
            TemperaturaSkladistenja = temperaturaSkladistenja;
            MaksimalanBrojPaleta = maksimalanBrojPaleta;
        }

        public TipPodrum Tip { get; set; }

        public enum TipPodrum
        {
            Vinski,
            Lokalni
        }
    }
}


