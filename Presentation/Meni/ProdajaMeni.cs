using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Meni
{
    public class ProdajaMeni
    {
        private readonly IProdajaServis prodajaServis;

        public ProdajaMeni(IProdajaServis prodajaServis)
        {
            this.prodajaServis = prodajaServis;
        }

        public void Prikazi()
        {
            Console.WriteLine("\n=== Isporuka vina kupcu ===");

            Guid paletaId = Guid.NewGuid();
            Console.WriteLine($"ID palete: {paletaId}");


            Console.Write("Unesi kupca (naziv/adresa): ");
            string? kupac = Console.ReadLine();

            Console.Write("Unesi cijenu po komadu: ");
            string? sCena = Console.ReadLine();

            if (!decimal.TryParse(sCena, out decimal cena) || cena <= 0)
            {
                Console.WriteLine("Neispravna cijena.");
                return;
            }

            try
            {
                Guid fakturaId = prodajaServis.IsporuciVinoKupcu(paletaId, kupac ?? "", cena);
                Console.WriteLine($"Isporuka uspješna. Faktura ID: {fakturaId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Greška: {ex.Message}");
            }

            Console.WriteLine("Enter za nastavak...");
            Console.ReadLine();
        }
    }
}