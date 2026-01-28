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

using Services.SkladistenjeServisi;

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

            
            IVinskiPodrumRepozitorijum vinskiPodrumRepozitorijum = new VinskiPodrumRepozitorijum(bazaPodataka);

            IFermentacijaRepozitorijum fermentacijaRepozitorijum = new FermentacijaRepozitorijum(bazaPodataka);
            IMerenjeSeceraRepozitorijum merenjeSeceraRepozitorijum = new MerenjeSeceraRepozitorijum(bazaPodataka);
            IEvidencijaProizvodnjeVinaRepozitorijum evidencijaVinaRepo = new EvidencijaProizvodnjeVinaRepozitorijum(bazaPodataka);

           
            IPaleteRepozitorijum paleteRepozitorijum = new PaleteRepozitorijum(bazaPodataka);

            IFermentacijaServis fermentacijaServis = new FermentacijaServis(fermentacijaRepozitorijum);
            IMerenjeSeceraServis merenjeSeceraServis = new MerenjeSeceraServis(merenjeSeceraRepozitorijum, fermentacijaRepozitorijum);
            IEvidencijaProizvodnjeVinaServis evidencijaVinaServis = new EvidencijaProizvodnjeVinaServis(evidencijaVinaRepo, fermentacijaRepozitorijum);

            IAutentifikacijaServis autentifikacijaServis = new AutentifikacioniServis(korisniciRepozitorijum);

            IBerbaLozeRepozitorijum berbaLozeRepo = new BerbaLozeRepozitorijum(bazaPodataka);


            
            ILoggerServis loggerServis = new LoggerServis();

            
            IPaleteServis paleteServis = new PaleteServis(paleteRepozitorijum, vinskiPodrumRepozitorijum, loggerServis, evidencijaVinaRepo);

            IBerbaLozeServis berbaLozeServis = new BerbaLozeServis(berbaLozeRepo, loggerServis);

            IProracunGrozdjaServis proracunGrozdjaServis = new ProracunGrozdjaServis();

            IVinovaLozaServis vinovaLozaServis = new VinovaLozaServis(vinovaLozaRepozitorijum);


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

            //Izbor skladistenja
            ISKladistenjeServis skladistenjeServis = new SkladistenjeServis();
            var skladMeni = new SkladistenjeMeni(skladistenjeServis);
            skladMeni.Prikazi();

            // ✅ IZMIJENJENO: dodali smo paleteMeni kao 4. parametar
            OpcijeMeni meni = new OpcijeMeni(fermentacijaServis, merenjeSeceraServis, evidencijaVinaServis, paleteMeni,berbaLozeServis, proracunGrozdjaServis, vinovaLozaServis);
            meni.PrikaziMeni();
        }
    }
}
