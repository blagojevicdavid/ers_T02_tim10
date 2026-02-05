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
            vina ??= new List<Vino>();

            var cista = vina
                .Where(v => v != null)
                .GroupBy(v => v.Id)
                .Select(g => g.First())
                .ToList();

            var ponuda = new Dictionary<Vino, StavkaFakture>();

            foreach (var v in cista)
            {
                decimal cena = IzracunajCenu(v.Kategorija, v.ZapreminaLitara);
                int kolicina = IzracunajKolicinu(v.ZapreminaLitara);

                ponuda[v] = new StavkaFakture(v.Id, kolicina, cena);
            }

            return ponuda;
        }

        private static int IzracunajKolicinu(double zapreminaLitra)
        {
            if (Math.Abs(zapreminaLitra - 0.75) < 0.01)
                return 24;

            if (Math.Abs(zapreminaLitra - 1.0) < 0.01)
                return 18;

            if (Math.Abs(zapreminaLitra - 1.5) < 0.01)
                return 12;

            return 10; // fallback
        }


        private static decimal IzracunajCenu(KategorijaVina kategorija, double zapreminaLitra)
        {
            bool velika = Math.Abs(zapreminaLitra - 1.5) < 0.0001;

            if (kategorija.ToString().Equals("Stolno", StringComparison.OrdinalIgnoreCase))
                return velika ? 810m : 450m;

            if (kategorija.ToString().Equals("Kvalitetno", StringComparison.OrdinalIgnoreCase))
                return velika ? 1170m : 650m;

            if (kategorija.ToString().Equals("Premium", StringComparison.OrdinalIgnoreCase))
                return velika ? 1620m : 900m;

            return velika ? 1000m : 500m;
        }
    }
}
