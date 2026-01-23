using Database.BazaPodataka;
using Database.BazaPodataka.Database.BazaPodataka;
using Database.Repozitorijumi;

using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

using Presentation.Authentifikacija;
using Presentation.Meni;

using Services.AutentifikacioniServisi;
using Services.VinogradServisi;
using Services.LoggerServisi; // mora odgovarati tvom namespace-u

namespace Loger_Bloger
{
    public class Program
    {
        public static void Main()
        {
            IBazaPodataka bazaPodataka = new XmlBazaPodataka();

            IKorisniciRepozitorijum korisniciRepozitorijum = new KorisniciRepozitorijum(bazaPodataka);
            IVinoRepozitorijum vinoRepozitorijum = new VinoRepozitorijum(bazaPodataka);
            IVinovaLozaRepozitorijum vinovaLozaRepozitorijum = new VinoveLozeRepozitorijum(bazaPodataka);

            // ✅ BITNO: tačno ime varijable koristimo kasnije
            IVinskiPodrumRepozitorijum vinskiPodrumRepozitorijum = new VinskiPodrumRepozitorijum(bazaPodataka);

            IFermentacijaRepozitorijum fermentacijaRepozitorijum = new FermentacijaRepozitorijum(bazaPodataka);
            IMerenjeSeceraRepozitorijum merenjeSeceraRepozitorijum = new MerenjeSeceraRepozitorijum(bazaPodataka);
            IEvidencijaProizvodnjeVinaRepozitorijum evidencijaVinaRepo = new EvidencijaProizvodnjeVinaRepozitorijum(bazaPodataka);

            // ✅ DODATO: palete repo
            IPaleteRepozitorijum paleteRepozitorijum = new PaleteRepozitorijum(bazaPodataka);

            IFermentacijaServis fermentacijaServis = new FermentacijaServis(fermentacijaRepozitorijum);
            IMerenjeSeceraServis merenjeSeceraServis = new MerenjeSeceraServis(merenjeSeceraRepozitorijum, fermentacijaRepozitorijum);
            IEvidencijaProizvodnjeVinaServis evidencijaVinaServis = new EvidencijaProizvodnjeVinaServis(evidencijaVinaRepo, fermentacijaRepozitorijum);

            IAutentifikacijaServis autentifikacijaServis = new AutentifikacioniServis(korisniciRepozitorijum);

            // ✅ DODATO: logger (ako se tvoja klasa ne zove LoggerServis, preimenuj OVDJE)
            ILoggerServis loggerServis = new LoggerServis();

            // ✅ DODATO: palete servis
            IPaleteServis paleteServis = new PaleteServis(paleteRepozitorijum, vinskiPodrumRepozitorijum, loggerServis);

            // ✅ DODATO: palete meni
            PaleteMeni paleteMeni = new PaleteMeni(paleteServis);

            AutentifikacioniMeni am = new AutentifikacioniMeni(autentifikacijaServis);
            Korisnik prijavljen = new Korisnik();

            while (am.TryLogin(out prijavljen) == false)
            {
                Console.WriteLine("Pogrešno korisničko ime ili lozinka. Pokušajte ponovo.");
            }

            Console.Clear();
            Console.WriteLine($"Uspešno ste prijavljeni kao: {prijavljen.ImePrezime} ({prijavljen.Uloga})");

            // ✅ IZMIJENJENO: dodali smo paleteMeni kao 4. parametar
            OpcijeMeni meni = new OpcijeMeni(fermentacijaServis, merenjeSeceraServis, evidencijaVinaServis, paleteMeni);
            meni.PrikaziMeni();
        }
    }
}
