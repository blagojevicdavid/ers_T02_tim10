using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Enumeracije;

namespace Domain.Modeli
{
    public class VinovaLoza
    {
        public Guid Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public double NivoSeceraBrix { get; set; }  
        public int GodinaSadnje { get; set; }
        public string RegionUzgoja { get; set; } = string.Empty;
        public FazaZrelostiLoze FazaZrelosti { get; set; }

        
        public VinovaLoza()
        {
            Id = Guid.NewGuid();
        }

        public VinovaLoza(Guid id, string naziv, double nivoSeceraBrix, int godinaSadnje, string regionUzgoja, FazaZrelostiLoze fazaZrelosti)
        {
            Id = id;
            Naziv = naziv;
            NivoSeceraBrix = nivoSeceraBrix;
            GodinaSadnje = godinaSadnje;
            RegionUzgoja = regionUzgoja;
            FazaZrelosti = fazaZrelosti;
        }
    }
}

