using Domain.Enumeracije;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Presentation.Meni
{
    public class VinskiPodrumMeni
    {
        private readonly IVinskiPodrumRepozitorijum vinskiPodrumRepo;
        private readonly ISkladistenjeServis skladistenjeServis;

        public VinskiPodrumMeni(
            IVinskiPodrumRepozitorijum vinskiPodrumRepo,
            ISkladistenjeServis skladistenjeServis)
        {
            this.vinskiPodrumRepo = vinskiPodrumRepo;
            this.skladistenjeServis = skladistenjeServis;
        }

        public void Prikazi()
        {
            if (skladistenjeServis.PreuzmiNacinSkladistenja() != NacinSkladistenja.VinskiPodrum)
                return;

            //da li je izabran način skladištenja
            var nacin = skladistenjeServis.PreuzmiNacinSkladistenja();

            if (nacin != NacinSkladistenja.VinskiPodrum)
            {
                Console.WriteLine("Izabrali ste lokalni podrum.");
                Console.WriteLine("Pritisni ENTER za nastavak");
                Console.ReadLine();
                return;
            }

            // prikaži listu podruma
            var podrumi = vinskiPodrumRepo.SviVinskiPodrumi().ToList();

            if (podrumi == null || podrumi.Count == 0)
            {
                Console.WriteLine("Nema dostupnih vinskih podruma u sistemu.");
                Console.WriteLine("Pritisni ENTER za nastavak");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Dostupni vinski podrumi:");
            foreach (var p in podrumi)
            {
                Console.WriteLine($"- {p.Id} | {p.Naziv}");
            }

            //Unos GUID-a
            Console.Write("Unesi ID vinskog podruma: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Neispravan GUID.");
                return;
            }

            // Provera da li postoji takav podrum
            var izabrani = vinskiPodrumRepo.PronadjiVinskiPodrumPoId(id);
            if (izabrani == null)
            {
                Console.WriteLine("Vinski podrum sa tim ID ne postoji.");
                return;
            }

            // Sačuvaj izbor
            skladistenjeServis.PostaviVinskiPodrum(id);

            Console.WriteLine($" Izabran vinski podrum: {izabrani.Naziv}");
            Console.WriteLine("Pritisni ENTER za nastavak");
            Console.ReadLine();
        }
    }
}
