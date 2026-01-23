using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Meni
{
    public class ProracunGrozdjaMeni
    {
        private readonly IProracunGrozdjaServis proracunServis;

        public ProracunGrozdjaMeni(IProracunGrozdjaServis proracunServis)
        {
            this.proracunServis = proracunServis;
        }

        public void Prikazi()
        {
            bool nazad = false;

            while (!nazad)
            {
                Console.WriteLine("\n--- PRORAČUN GROŽĐA ---");
                Console.WriteLine("1) Izračunaj potrebnu količinu loza"); 
                Console.WriteLine("0) Nazad");
                Console.Write("Izbor: ");

                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        Izracunaj();
                        break;

                    case "0":
                        nazad = true;
                        break;

                    default:
                        Console.WriteLine("Nepoznata opcija.");
                        break;
                }
            }
        }

        private void Izracunaj()
        {
            Console.Write("Unesi broj flaša: ");
            if (!int.TryParse(Console.ReadLine(), out int brojFlasa) || brojFlasa <= 0)
            {
                Console.WriteLine("Neispravan broj flaša.");
                return;
            }

            Console.Write("Unesi zapreminu jedne flaše (L): ");
            if (!double.TryParse(Console.ReadLine(), out double zapremina) || zapremina <= 0)
            {
                Console.WriteLine("Neispravna zapremina.");
                return;
            }

            int potrebneLoze = proracunServis.IzracunajPotrebnuKolicinuLoza(brojFlasa, zapremina);

            Console.WriteLine($"Potrebno loza: {potrebneLoze}");
        }
    }
}
