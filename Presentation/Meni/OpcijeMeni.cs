using System;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        private readonly IEvidencijaProizvodnjeVinaServis evidencijaVinaServis;
<<<<<<< HEAD
        private readonly PaleteMeni paleteMeni;
        private readonly PakovanjeMeni pakovanjeMeni;
=======
        

        // ✅ DODATO
        private readonly PaleteMeni paleteMeni;
        private readonly IBerbaLozeServis berbaLozeServis;
        private readonly IProracunGrozdjaServis proracunGrozdjaServis;
        private readonly IVinovaLozaServis vinovaLozaServis;
>>>>>>> 7d356f703eacf9bf164455f1f8b479e0d8615f72

        public OpcijeMeni(
            IEvidencijaProizvodnjeVinaServis evidencijaVinaServis,
            PaleteMeni paleteMeni,
<<<<<<< HEAD
            PakovanjeMeni pakovanjeMeni)
=======
            IBerbaLozeServis berbaLozeServis,
            IProracunGrozdjaServis proracunGrozdjaServis,
            IVinovaLozaServis vinovaLozaServis
            /*ovde navesti ostale servise*/)
>>>>>>> 7d356f703eacf9bf164455f1f8b479e0d8615f72
        {
            this.evidencijaVinaServis = evidencijaVinaServis;
            this.paleteMeni = paleteMeni;
<<<<<<< HEAD
            this.pakovanjeMeni = pakovanjeMeni;
=======

            // i ovde ispuniti za ostale
            this.berbaLozeServis = berbaLozeServis;
            this.proracunGrozdjaServis = proracunGrozdjaServis;
            this.vinovaLozaServis = vinovaLozaServis;

>>>>>>> 7d356f703eacf9bf164455f1f8b479e0d8615f72
        }

        public void Prikazi()
        {
<<<<<<< HEAD
            bool izlaz = false;
=======
            Console.WriteLine("\n============================================ Meni ===========================================");
            Console.WriteLine("Odaberite jednu od sledećih opcija:");
            Console.WriteLine("1) Meni fermentacije");
            Console.WriteLine("2) Proizvodnja vina (gotovi proizvodi)");
            Console.WriteLine("3) Berba loze");
            Console.WriteLine("4) Proračun grožđa");
            Console.WriteLine("5) Sadnja vinove loze");
            Console.WriteLine("0) Izlaz");
>>>>>>> 7d356f703eacf9bf164455f1f8b479e0d8615f72

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
<<<<<<< HEAD
                        new ProizvodnjaVinaMeni(
                            evidencijaVinaServis,
                            paleteMeni,
                            pakovanjeMeni
                        ).Prikazi();
                        break;

                    case "0":
                        izlaz = true;
=======
                        new FermentacijaMeni(fermentacijaServis, merenjeSeceraServis /*ovde ostatak servis*/).Prikazi();
                        Console.WriteLine("\n============================================ Meni ===========================================");
                        Console.WriteLine("Odaberite jednu od sledećih opcija:");
                        Console.WriteLine("1) Meni fermentacije");
                        Console.WriteLine("2) Proizvodnja vina (gotovi proizvodi)");
                        Console.WriteLine("3) Berba loze");
                        Console.WriteLine("4) Proračun grožđa");
                        Console.WriteLine("5) Sadnja vinove loze");
                        // ovde dodati ostale menije


                        Console.WriteLine("0) Izlaz");
                        break;

                    case "0":
                        kraj = true;
                        break;

                    case "2":
                        // ✅ MINIMALNO: sada prosleđujemo paleteMeni u “gotov proizvod”
                        new ProizvodnjaVinaMeni(evidencijaVinaServis, paleteMeni).Prikazi();

                        Console.WriteLine("\n============================================ Meni ===========================================");
                        Console.WriteLine("Odaberite jednu od sledećih opcija:");
                        Console.WriteLine("1) Meni fermentacije");
                        Console.WriteLine("2) Proizvodnja vina (gotovi proizvodi)");
                        Console.WriteLine("3) Berba loze");
                        Console.WriteLine("4) Proračun grožđa");
                        Console.WriteLine("5) Sadnja vinove loze");
                        // ovde dodati ostale menije
                        Console.WriteLine("0) Izlaz");
                        break;

                    case "3":
                        new BerbaLozeMeni(berbaLozeServis).Prikazi();

                        Console.WriteLine("\n============================================ Meni ===========================================");
                        Console.WriteLine("Odaberite jednu od sledećih opcija:");
                        Console.WriteLine("1) Meni fermentacije");
                        Console.WriteLine("2) Proizvodnja vina (gotovi proizvodi)");
                        Console.WriteLine("3) Berba loze");
                        Console.WriteLine("4) Proračun grožđa");
                        Console.WriteLine("5) Sadnja vinove loze");
                        Console.WriteLine("0) Izlaz");
                        break;

                    case "4":
                        new ProracunGrozdjaMeni(proracunGrozdjaServis).Prikazi();

                        Console.WriteLine("\n============================================ Meni ===========================================");
                        Console.WriteLine("Odaberite jednu od sledećih opcija:");
                        Console.WriteLine("1) Meni fermentacije");
                        Console.WriteLine("2) Proizvodnja vina (gotovi proizvodi)");
                        Console.WriteLine("3) Berba loze");
                        Console.WriteLine("4) Proračun grožđa");
                        Console.WriteLine("5) Sadnja vinove loze");
                        Console.WriteLine("0) Izlaz");
                        break;

                    case "5":
                        new VinovaLozaMeni(vinovaLozaServis).Prikazi();
                        Console.WriteLine("\n============================================ Meni ===========================================");
                        Console.WriteLine("Odaberite jednu od sledećih opcija:");
                        Console.WriteLine("1) Meni fermentacije");
                        Console.WriteLine("2) Proizvodnja vina (gotovi proizvodi)");
                        Console.WriteLine("3) Berba loze");
                        Console.WriteLine("4) Proračun grožđa");
                        Console.WriteLine("5) Sadnja vinove loze");
                        Console.WriteLine("0) Izlaz");

                        break;

                    default:
                        Console.WriteLine("Nepoznata opcija. Pokusaj ponovo.");
>>>>>>> 7d356f703eacf9bf164455f1f8b479e0d8615f72
                        break;
                }
            }
        }
    }
}
