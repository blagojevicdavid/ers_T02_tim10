using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Meni
{
    public class VinovaLozaMeni
    {
        private readonly IVinovaLozaServis vinovaLozaServis;

        public VinovaLozaMeni(IVinovaLozaServis vinovaLozaServis)
        {
            this.vinovaLozaServis = vinovaLozaServis;
        }

        public void Prikazi()
        {
            bool nazad = false;

            while (!nazad)
            {
                Console.WriteLine("\n--- VINOVA LOZA (SADNJA) ---");
                Console.WriteLine("1) Zasadi lozu (detaljno)");
                Console.WriteLine("2) Zasadi lozu (samo naziv)");
                Console.WriteLine("0) Nazad");
                Console.Write("Izbor: ");

                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        ZasadiDetaljno();
                        break;
                    case "2":
                        ZasadiSamoNaziv();
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

        private void ZasadiSamoNaziv()
        {
            Console.Write("Unesi naziv sorte: ");
            string? naziv = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(naziv))
            {
                Console.WriteLine("Naziv ne sme biti prazan.");
                return;
            }

            var loza = vinovaLozaServis.ZasadiLozu(naziv.Trim());
            Console.WriteLine("Sadnja evidentirana.");
        }

        private void ZasadiDetaljno()
        {
            Console.Write("Naziv sorte: ");
            string? naziv = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(naziv))
            {
                Console.WriteLine("Naziv ne sme biti prazan.");
                return;
            }

            Console.Write("Nivo šećera (Brix): ");
            if (!double.TryParse(Console.ReadLine(), out double brix))
            {
                Console.WriteLine("Neispravan broj za Brix.");
                return;
            }

            Console.Write("Godina sadnje: ");
            if (!int.TryParse(Console.ReadLine(), out int godina))
            {
                Console.WriteLine("Neispravna godina.");
                return;
            }

            Console.Write("Region: ");
            string? region = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(region))
            {
                Console.WriteLine("Region ne sme biti prazan.");
                return;
            }

            var loza = vinovaLozaServis.ZasadiLozu(naziv.Trim(), brix, godina, region.Trim());
            Console.WriteLine("Sadnja evidentirana.");
        }
    }
}
