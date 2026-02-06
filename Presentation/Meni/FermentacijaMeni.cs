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
        //servisi 
        private readonly IFermentacijaServis fermentacijaServis;
        private readonly IMerenjeSeceraServis merenjeSeceraServis;

        public FermentacijaMeni(IFermentacijaServis servis, IMerenjeSeceraServis merenjeServis)
        {
            fermentacijaServis = servis;
            merenjeSeceraServis = merenjeServis;
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
                Console.WriteLine("5) Dodaj merenje šećera (Brix)");
                Console.WriteLine("6) Pregled merenja šećera (Brix)");
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
                    case "5":
                        DodajMerenjeSecera();
                        break;

                    case "6":
                        PregledMerenjaSecera();
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
                    $"ID={f.Id} | BerbaId={f.BerbaId} | Faza={f.Faza} | Pocetak={f.DatumPocetka} | Kraj={f.DatumZavrsetka}");
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
            Console.WriteLine($"LozaId: {f.BerbaId}");
            Console.WriteLine($"Faza: {f.Faza}");
            Console.WriteLine($"Datum pocetka: {f.DatumPocetka}");
            Console.WriteLine($"Datum zavrsetka: {f.DatumZavrsetka}");
            //Console.WriteLine($"Napomena: {f.Napomena}");
        }

        private void Zapocni()
        {
            Console.Write("Unesi ID berbe: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid berbaId))
            {
                Console.WriteLine("Neispravan GUID.");
                return;
            }

            var nova = fermentacijaServis.ZapocniFermentaciju(berbaId);
            Console.WriteLine($"Započeta fermentacija: ID={nova.Id}, Faza={nova.Faza}");
        }

        private void PromeniFazu()
        {
            Console.Write("Unesi Fermentacija Id (GUID): ");
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

        private void DodajMerenjeSecera()
        {
            Console.Write("Unesi FermentacijaId (GUID): ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid fid))
            {
                Console.WriteLine("Neispravan GUID.");
                return;
            }

            Console.Write("Unesi nivo šećera (Brix): ");
            if (!double.TryParse(Console.ReadLine(), out double brix))
            {
                Console.WriteLine("Neispravan broj.");
                return;
            }

            Console.Write("Napomena (opciono): ");
            string? napomena = Console.ReadLine();

            try
            {
                var m = merenjeSeceraServis.DodajMerenje(fid, brix, napomena ?? "");
                Console.WriteLine($"Upisano merenje: {m.NivoSeceraBrix} Brix u {m.DatumVreme} (ID={m.Id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
            }
        }

        private void PregledMerenjaSecera()
        {
            Console.Write("Unesi FermentacijaId (GUID): ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid fid))
            {
                Console.WriteLine("Neispravan GUID.");
                return;
            }

            var lista = merenjeSeceraServis.PregledMerenja(fid);

            Console.WriteLine("\n--- Merenja šećera (Brix) ---");
            int brojac = 0;
            foreach (var m in lista)
            {
                brojac++;
                Console.WriteLine($"{brojac}) {m.DatumVreme} | {m.NivoSeceraBrix} Brix | {m.Napomena}");
            }

            if (brojac == 0)
                Console.WriteLine("Nema merenja za ovu fermentaciju.");
        }

    }
}
