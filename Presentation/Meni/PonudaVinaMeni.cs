using Domain.Modeli;
using Domain.PomocneMetode;
using Domain.Servisi;
using System;

namespace Presentation.Meni
{
    public class PonudaVinaMeni
    {
        private readonly IPonudaVinaServis ponudaServis;

        public PonudaVinaMeni(IPonudaVinaServis ponudaServis)
        {
            this.ponudaServis = ponudaServis;
        }

        public void Prikazi()
        {
            Console.Clear();

            Console.WriteLine(new string('=', 95));
            Console.WriteLine("                 P R E G L E D   P O N U D E   V I N A");
            Console.WriteLine(new string('=', 95));
            Console.WriteLine();

            var vina = ponudaServis.VratiPonudu();
            if (vina == null || vina.Count == 0)
            {
                Console.WriteLine("Nema vina u ponudi.");
                Console.ReadLine();
                return;
            }

            var ponuda = PonudaVinaPomocne.FormirajPonudu(vina);

            // zaglavlje tabele
            Console.WriteLine(
                $"{"Naziv",-30} | {"Kategorija",-14} | {"Zapremina",-9} | {"Cena",-6} | {"Kolicina",-8}"
            );
            Console.WriteLine(new string('-', 95));

            foreach (var kv in ponuda)
            {
                Vino v = kv.Key;
                StavkaFakture s = kv.Value;

                string zap = v.ZapreminaLitara.ToString("0.##").Replace('.', ',');

                Console.WriteLine(
                    $"{Skrati(v.Naziv, 30),-30} | {v.Kategorija,-14} | {zap,-9} | {s.CenaPoKomadu,-6} | {s.Kolicina,-8}"
                );
            }

            Console.WriteLine(new string('-', 95));
            Console.ReadLine(); 
        }
        private static string Skrati(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return "";
            if (s.Length <= max)
            {
                return s;
            }
            return s.Substring(0, max - 3) + "...";
        }
    }
}
