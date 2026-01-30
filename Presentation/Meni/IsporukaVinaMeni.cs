using System;
using Domain.Modeli;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class IsporukaVinaMeni
    {
        private readonly IIsporukaVinaServis isporukaVinaServis;

        public IsporukaVinaMeni(IIsporukaVinaServis isporukaVinaServis)
        {
            this.isporukaVinaServis = isporukaVinaServis;
        }

        public void Prikazi()
        {
            Console.WriteLine("\n--- ISPORUKA VINA (Zahtjev za isporuku) ---");

            Console.Write("Unesi broj paleta za isporuku: ");
            if (!int.TryParse(Console.ReadLine(), out int brojPaleta) || brojPaleta <= 0)
            {
                Console.WriteLine("Neispravan broj paleta.");
                return;
            }

            var zahtjev = new ZahtjevZaIsporuku
            {
                BrojPaleta = brojPaleta
            };

            //rememberStatus("Kreiran", zahtjev); treba mi kasnije

            isporukaVinaServis.PosaljiZahtjev(zahtjev);

            Console.WriteLine($"\nStatus zahtjeva: {zahtjev.Status}");
            Console.WriteLine($"Zahtjev ID: {zahtjev.Id}");
            Console.WriteLine($"Vrijeme: {zahtjev.VrijemeZahtjeva}");
        }

    }
}
