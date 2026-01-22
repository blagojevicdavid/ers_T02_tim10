using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Domain.Enumeracije;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class FermentacijaMeni
    {
        private readonly IFermentacijaServis fermentacijaServis;

        public FermentacijaMeni(IFermentacijaServis servis)
        {
            fermentacijaServis = servis;
        }

        public void Prikazi()
        {
            bool nazad = false;

            while (!nazad)
            {
                Console.WriteLine("\n--- FERMENTACIJA (Praćenje) ---");
                Console.WriteLine("1) Pregled svih fermentacija");
                Console.WriteLine("2) Pregled jedne fermentacije");
                Console.WriteLine("3) Započni fermentaciju");
                Console.WriteLine("4) Promeni fazu fermentacije");
                Console.WriteLine("0) Nazad");
                Console.Write("Izbor: ");

                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        PregledSvih();
                        break;

                    case "2":
                        PregledJedne();
                        break;

                    case "3":
                        Zapocni();
                        break;

                    case "4":
                        PromeniFazu();
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

        private void PregledSvih()
        {
            var sve = fermentacijaServis.PregledSvihFermentacija();

            Console.WriteLine("\n--- Lista fermentacija ---");
            foreach (var f in sve)
            {
                Console.WriteLine(
                    $"ID={f.Id} | LozaId={f.LozaId} | Faza={f.Faza} | Pocetak={f.DatumPocetka} | Kraj={f.DatumZavrsetka}");
            }
        }

        private void PregledJedne()
        {
            Console.Write("Unesi FermentacijaId (GUID): ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid fid))
            {
                Console.WriteLine("Neispravan GUID.");
                return;
            }

            var f = fermentacijaServis.PregledFermentacije(fid);
            if (f == null || f.Id == Guid.Empty)
            {
                Console.WriteLine("Fermentacija nije pronađena.");
                return;
            }

            Console.WriteLine("\n--- Detalji fermentacije ---");
            Console.WriteLine($"ID: {f.Id}");
            Console.WriteLine($"LozaId: {f.LozaId}");
            Console.WriteLine($"Faza: {f.Faza}");
            Console.WriteLine($"Datum pocetka: {f.DatumPocetka}");
            Console.WriteLine($"Datum zavrsetka: {f.DatumZavrsetka}");
            Console.WriteLine($"Napomena: {f.Napomena}");
        }

        private void Zapocni()
        {
            Console.Write("Unesi LozaId (GUID): ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid lozaId))
            {
                Console.WriteLine("Neispravan GUID.");
                return;
            }

            var nova = fermentacijaServis.ZapocniFermentaciju(lozaId);
            Console.WriteLine($"Započeta fermentacija: ID={nova.Id}, Faza={nova.Faza}");
        }

        private void PromeniFazu()
        {
            Console.Write("Unesi FermentacijaId (GUID): ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid fid))
            {
                Console.WriteLine("Neispravan GUID.");
                return;
            }

            Console.WriteLine("Izaberi fazu:");
            Console.WriteLine("0 - Kreirana");
            Console.WriteLine("1 - Pokrenuta");
            Console.WriteLine("2 - Aktivna");
            Console.WriteLine("3 - Zavrsena");
            Console.WriteLine("4 - Prekinuta");
            Console.Write("Faza: ");

            if (!int.TryParse(Console.ReadLine(), out int fazaInt))
            {
                Console.WriteLine("Neispravan unos.");
                return;
            }

            if (!Enum.IsDefined(typeof(FazaFermentacije), fazaInt))
            {
                Console.WriteLine("Nepostojeca faza.");
                return;
            }

            var faza = (FazaFermentacije)fazaInt;

            bool ok = fermentacijaServis.PromeniFazu(fid, faza);
            Console.WriteLine(ok ? "Faza ažurirana." : "Fermentacija nije pronađena / greška.");
        }
    }
}
