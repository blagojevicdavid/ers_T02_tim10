using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Meni
{
    public class BerbaLozeMeni
    {
        private readonly IBerbaLozeServis _berbaServis;

        public BerbaLozeMeni(IBerbaLozeServis berbaServis)
        {
            _berbaServis = berbaServis;
        }

        public void Prikazi()
        {
            bool izlaz = false;

            while (!izlaz)
            {
                Console.WriteLine("\n--- BERBA LOZE ---");
                Console.WriteLine("1) Evidentiraj berbu");
                Console.WriteLine("2) Prikaži sve berbe");
                Console.WriteLine("0) Nazad");
                Console.Write("Izbor: ");

                string izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        Evidentiraj();
                        break;
                    case "2":
                        PrikaziSve();
                        break;
                    case "0":
                        izlaz = true;
                        break;
                    default:
                        Console.WriteLine("Nepoznata opcija.");
                        break;
                }
            }
        }

        private void Evidentiraj()
        {
            Console.Write("Unesi datum berbe (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime datum))
            {
                Console.WriteLine("Neispravan datum.");
                return;
            }

            Console.Write("Unesi količinu (kg): ");
            if (!double.TryParse(Console.ReadLine(), out double kg))
            {
                Console.WriteLine("Neispravna količina.");
                return;
            }

            var sacuvana = _berbaServis.EvidentirajBerbu(datum, kg);

            if (sacuvana == null || sacuvana.Id == Guid.Empty)
                Console.WriteLine("Greška: berba nije evidentirana.");
            else
                Console.WriteLine($"Evidentirano: ID={sacuvana.Id}, datum={sacuvana.DatumBerbe:yyyy-MM-dd}, kg={sacuvana.KolicinaKg}");
        }

        private void PrikaziSve()
        {
            var sve = _berbaServis.VratiSveBerbe();

            Console.WriteLine("\n--- SVE BERBE ---");
            foreach (var b in sve)
                Console.WriteLine($"ID={b.Id} | Datum={b.DatumBerbe:yyyy-MM-dd} | Kg={b.KolicinaKg}");
        }
    }
}
