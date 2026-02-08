using Domain.Enumeracije;
using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.PomocneMetode
{
    public static class PonudaVinaPomocne
    {
        public static Dictionary<Vino, StavkaFakture> FormirajPonudu(List<Vino> vina)
        {
            if (vina == null)
            {
                vina = new List<Vino>();
            }

            var cista = vina
                .Where(v => v != null)
                .GroupBy(v => v.Id)
                .Select(g => g.First())
                .ToList();

            var ponuda = new Dictionary<Vino, StavkaFakture>();

            foreach (var v in cista)
            {
                decimal cena = IzracunajCenu(v.Kategorija, v.ZapreminaLitara);
                int kolicina = IzracunajKolicinu(v.ZapreminaLitara, v.Naziv, v.Kategorija);
                ponuda[v] = new StavkaFakture(v.Id, kolicina, cena);
            }

            return ponuda;
        }


        private static int IzracunajKolicinu(double zapreminaLitra, string naziv, KategorijaVina kategorija)
        {
            if (kategorija.ToString() == "Premium" && Math.Abs(zapreminaLitra - 0.75) < 0.01)
                return 0;

            if (naziv.Contains("Reserve"))
                return 0;

            if (Math.Abs(zapreminaLitra - 0.75) < 0.01)
                return 24;

            if (Math.Abs(zapreminaLitra - 1.0) < 0.01)
                return 18;

            if (Math.Abs(zapreminaLitra - 1.5) < 0.01)
                return 12;

            return 10;
        }


        private static decimal IzracunajCenu(KategorijaVina kategorija, double zapreminaLitra)
        {
            bool velika = Math.Abs(zapreminaLitra - 1.5) < 0.0001;

            if (kategorija == KategorijaVina.Stolno)
            {
                if (velika) return 810m;
                return 450m;
            }

            if (kategorija == KategorijaVina.Kvalitetno)
            {
                if (velika) return 1170m;
                return 650m;
            }

            if (kategorija == KategorijaVina.Premium)
            {
                if (velika) return 1620m;
                return 900m;
            }

            if (velika) return 1000m;
            return 500m;
        }

    }
}
