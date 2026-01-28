using Domain.Enumeracije;
using Domain.Servisi;
using Services.SkladistenjeServisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Meni
{
    public class SkladistenjeMeni
    {
        private readonly ISKladistenjeServis skladistenjeServis;

        public SkladistenjeMeni(ISKladistenjeServis skladistenjeServis)
        {
            this.skladistenjeServis = skladistenjeServis;
        }

        public void Prikazi()
        {
            Console.WriteLine("\n ODABIR NAČINA SKLADIŠTENJA ");
            Console.WriteLine("1) Vinski podrum");
            Console.WriteLine("2) Lokalni podrum");
            Console.Write("Izbor: ");

            string? izbor = Console.ReadLine();

            if (izbor == "1")
                skladistenjeServis.PostaviNacinSkladistenja(NacinSkladistenja.VinskiPodrum);
            else if (izbor == "2")
                skladistenjeServis.PostaviNacinSkladistenja(NacinSkladistenja.LokalniPodrum);
            else
            {
                Console.WriteLine("Neispravan izbor.");
                return;
            }

            Console.WriteLine("Način skladištenja je uspešno izabran.");
        }
    }
}
