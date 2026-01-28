using Domain.Enumeracije;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Services.SkladistenjeServisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Meni
{
    public class LokalniPodrumMeni
    {
        private readonly IVinskiPodrumRepozitorijum vinskiPodrumRepo;
        private readonly ISKladistenjeServis skladistenjeServis;

        public LokalniPodrumMeni(
            IVinskiPodrumRepozitorijum lokalniPodrumRepo,
            ISKladistenjeServis skladistenjeServis)
        {
            this.vinskiPodrumRepo = lokalniPodrumRepo;
            this.skladistenjeServis = skladistenjeServis;
        }

        public void Prikazi()
        {
            if (skladistenjeServis.PreuzmiNacinSkladistenja() != NacinSkladistenja.LokalniPodrum)
                return;
            var nacin = skladistenjeServis.PreuzmiNacinSkladistenja();

            if (nacin != NacinSkladistenja.LokalniPodrum)
            {
                Console.WriteLine("Izabrali ste vinski podrum.");
                Console.WriteLine("Pritisni ENTER za nastavak");
                Console.ReadLine();
                return;
            }

            var podrumi = vinskiPodrumRepo.SviVinskiPodrumi().Where(p => p.Tip == Domain.Modeli.VinskiPodrum.TipPodrum.Lokalni).ToList();

            if (podrumi.Count == 0)
            {
                Console.WriteLine("Nema dostupnih lokalnih podruma u sistemu.");
                Console.WriteLine("Pritisni ENTER za nastavak");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Dostupni lokalni podrumi:");
            foreach (var p in podrumi)
            {
                Console.WriteLine($"- {p.Id} | {p.Naziv}");
            }

            Console.Write("Unesi ID lokalnog podruma: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Neispravan GUID.");
                return;
            }

            var izabrani = vinskiPodrumRepo.PronadjiVinskiPodrumPoId(id);
            if (izabrani == null)
            {
                Console.WriteLine("Lokalni podrum sa tim ID ne postoji.");
                return;
            }

            skladistenjeServis.PostaviLokalniPodrum(id);

            Console.WriteLine($" Izabran lokalni podrum: {izabrani.Naziv}");
            Console.WriteLine("Pritisni ENTER za nastavak");
            Console.ReadLine();
        }
    }
}
