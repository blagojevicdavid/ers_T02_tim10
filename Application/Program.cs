using System;
using Database.BazaPodataka;
using Database.Repozitorijumi;
using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Presentation.Authentifikacija;
using Presentation.Meni;
using Services.AutentifikacioniServisi;
using Services.LoggerServisi;
using Services.PakovanjeServisi;
using Services.SkladistenjeServisi;
using Services.VinogradServisi;
using Services.IsporukaServis;

namespace Loger_Bloger
{
    public class Program
    {
        public static void Main()
        {
            IBazaPodataka bazaPodataka = new XmlBazaPodataka("podaci.xml");

            // -------------------- REPOZITORIJUMI --------------------
            IKorisniciRepozitorijum korisniciRepozitorijum = new KorisniciRepozitorijum(bazaPodataka);
            IVinoRepozitorijum vinoRepozitorijum = new VinoRepozitorijum(bazaPodataka);
            IVinovaLozaRepozitorijum vinovaLozaRepozitorijum = new VinoveLozeRepozitorijum(bazaPodataka);

            IVinskiPodrumRepozitorijum vinskiPodrumRepozitorijum = new VinskiPodrumRepozitorijum(bazaPodataka);

            IFermentacijaRepozitorijum fermentacijaRepozitorijum = new FermentacijaRepozitorijum(bazaPodataka);
            IMerenjeSeceraRepozitorijum merenjeSeceraRepozitorijum = new MerenjeSeceraRepozitorijum(bazaPodataka);
            IEvidencijaProizvodnjeVinaRepozitorijum evidencijaVinaRepozitorijum =
                new EvidencijaProizvodnjeVinaRepozitorijum(bazaPodataka);

            IPaleteRepozitorijum paleteRepozitorijum = new PaleteRepozitorijum(bazaPodataka);
            IFaktureRepozitorijum faktureRepozitorijum = new FaktureRepozitorijum(bazaPodataka);

            IBerbaLozeRepozitorijum berbaLozeRepozitorijum = new BerbaLozeRepozitorijum(bazaPodataka);

            // -------------------- POCETNI PODACI --------------------
            PocetniPodaci.UbaciInicijalnePodatke(
                korisniciRepozitorijum,
                vinovaLozaRepozitorijum,
                vinoRepozitorijum,
                paleteRepozitorijum,
                faktureRepozitorijum,
                vinskiPodrumRepozitorijum
            );
            bazaPodataka.SacuvajPromene();

            // -------------------- SERVISI --------------------
            ILoggerServis loggerServis = new LoggerServis();

            IFermentacijaServis fermentacijaServis = new FermentacijaServis(fermentacijaRepozitorijum);
            IMerenjeSeceraServis merenjeSeceraServis =
                new MerenjeSeceraServis(merenjeSeceraRepozitorijum, fermentacijaRepozitorijum);

            IEvidencijaProizvodnjeVinaServis evidencijaVinaServis =
                new EvidencijaProizvodnjeVinaServis(evidencijaVinaRepozitorijum, fermentacijaRepozitorijum);

            IAutentifikacijaServis autentifikacijaServis = new AutentifikacioniServis(korisniciRepozitorijum);

            
            IPaleteServis paleteServis = new PaleteServis(
                paleteRepozitorijum,
                vinskiPodrumRepozitorijum,
                loggerServis,
                evidencijaVinaRepozitorijum
            );

            IBerbaLozeServis berbaLozeServis = new BerbaLozeServis(berbaLozeRepozitorijum, loggerServis);
            IProracunGrozdjaServis proracunGrozdjaServis = new ProracunGrozdjaServis();
            IVinovaLozaServis vinovaLozaServis = new VinovaLozaServis(vinovaLozaRepozitorijum);

            // -------------------- MENIJI --------------------
            PaleteMeni paleteMeni = new PaleteMeni(paleteServis);

            // -------------------- LOGIN --------------------
            AutentifikacioniMeni am = new AutentifikacioniMeni(autentifikacijaServis);
            Korisnik prijavljen;

            while (!am.TryLogin(out prijavljen))
            {
                Console.WriteLine("Pogrešno korisničko ime ili lozinka. Pokušajte ponovo.");
            }

            Console.Clear();
            Console.WriteLine($"Uspešno ste prijavljeni kao: {prijavljen.ImePrezime} ({prijavljen.Uloga})");

            // -------------------- SKLADISTENJE + PAKOVANJE --------------------
            ISkladistenjeServis skladistenjeServis;

            if (prijavljen.Uloga == TipKorisnika.GlavniEnolog)
                skladistenjeServis = new VinskiPodrumSkladistenjeServis(paleteRepozitorijum, loggerServis);
            else
                skladistenjeServis = new LokalniKelarSkladistenjeServis(paleteRepozitorijum, loggerServis);

            IPakovanjeServis pakovanjeServis =
                new PakovanjeServis(vinoRepozitorijum, paleteRepozitorijum, skladistenjeServis, loggerServis);

            IIsporukaVinaServis isporukaVinaServis = new IsporukaVinaServis(skladistenjeServis, loggerServis);



            PakovanjeMeni pakovanjeMeni = new PakovanjeMeni(pakovanjeServis);

            // -------------------- GLAVNI MENI --------------------
            OpcijeMeni meni = new OpcijeMeni(
                evidencijaVinaServis,
                paleteMeni,
                pakovanjeMeni,
                berbaLozeServis,
                proracunGrozdjaServis,
                vinovaLozaServis,
                isporukaVinaServis
            );

            meni.Prikazi();
        }
    }
}
