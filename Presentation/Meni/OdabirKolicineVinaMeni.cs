using Domain.Modeli;
using Domain.PomocneMetode;
using Domain.Servisi;
using System;
using System.Linq;

namespace Presentation.Meni
{
    public class OdabirKolicineVinaMeni
    {
        private readonly IPonudaVinaServis ponudaVinaServis;
        private readonly IOdabirKolicineVinaServis odabirKolicineServis;

        public OdabirKolicineVinaMeni(
            IPonudaVinaServis ponudaVinaServis,
            IOdabirKolicineVinaServis odabirKolicineServis)
        {
            this.ponudaVinaServis = ponudaVinaServis;
            this.odabirKolicineServis = odabirKolicineServis;
        }

        public void Prikazi()
        {
            Console.Clear();

            var svaVina = ponudaVinaServis.VratiPonudu();
            if (svaVina == null || svaVina.Count == 0)
            {
                Console.WriteLine("Nema vina u ponudi.");
                Console.ReadLine();
                return;
            }

           
            var mapa = PonudaVinaPomocne.FormirajPonudu(svaVina);

            var lista = mapa
                .Where(kv => kv.Value.Kolicina > 0)
                .Select(kv => kv.Key)
                .OrderBy(v => v.Naziv)
                .ToList();


            Console.WriteLine("                   --  D O S T U P N A   V I N A  --");

            Console.WriteLine();

            if (lista.Count == 0)
            {
                Console.WriteLine("Trenutno nema vina na stanju.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"{"RB",-3} | {"Naziv",-30} | {"Kategorija",-14} | {"Zapremina",-9}");
            Console.WriteLine(new string('-', 85));

            for (int i = 0; i < lista.Count; i++)
            {
                var v = lista[i];
                string zap = v.ZapreminaLitara.ToString("0.##").Replace('.', ',');

                Console.WriteLine($"{i + 1,-3} | {Skrati(v.Naziv, 30),-30} | {v.Kategorija,-14} | {zap,-9}");
            }

            Console.WriteLine(new string('-', 85));
        }
                      

        private static string Skrati(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return ""; if (s.Length <= max)
            {
                return s;
            }

            return s.Substring(0, max - 3) + "...";

        }
    }
}
