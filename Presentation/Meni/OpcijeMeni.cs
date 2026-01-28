using System;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        private readonly IEvidencijaProizvodnjeVinaServis evidencijaVinaServis;
        private readonly PaleteMeni paleteMeni;
        private readonly PakovanjeMeni pakovanjeMeni;

        public OpcijeMeni(
            IEvidencijaProizvodnjeVinaServis evidencijaVinaServis,
            PaleteMeni paleteMeni,
            PakovanjeMeni pakovanjeMeni)
        {
            this.evidencijaVinaServis = evidencijaVinaServis;
            this.paleteMeni = paleteMeni;
            this.pakovanjeMeni = pakovanjeMeni;
        }

        public void Prikazi()
        {
            bool izlaz = false;

            while (!izlaz)
            {
                Console.Clear();
                Console.WriteLine("=== GLAVNI MENI ===");
                Console.WriteLine("1) Proizvodnja vina");
                Console.WriteLine("0) Izlaz");
                Console.Write("Izbor: ");

                string izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        new ProizvodnjaVinaMeni(
                            evidencijaVinaServis,
                            paleteMeni,
                            pakovanjeMeni
                        ).Prikazi();
                        break;

                    case "0":
                        izlaz = true;
                        break;
                }
            }
        }
    }
}
