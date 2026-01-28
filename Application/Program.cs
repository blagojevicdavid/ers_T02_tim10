using System;
using Database.BazaPodataka;
using Database.Repozitorijumi;
using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
<<<<<<< HEAD
=======
using Domain.Enumeracije;

>>>>>>> 7d356f703eacf9bf164455f1f8b479e0d8615f72
using Presentation.Authentifikacija;
using Presentation.Meni;
using Services.AutentifikacioniServisi;
using Services.LoggerServisi;
using Services.PakovanjeServisi;
using Services.SkladistenjeServisi;
using Services.VinogradServisi;

using Services.SkladistenjeServisi;

namespace Loger_Bloger
{
    public class Program
    {
        public static void Main()
        {
            IBazaPodataka bazaPodataka = new XmlBazaPodataka("podaci.xml");

            IKorisniciRepozitorijum korisniciRepozitorijum = new KorisniciRepozitorijum(bazaPodataka);
            IVinoRepozitorijum vinoRepozitorijum = new VinoRepozitorijum(bazaPodataka);
            IVinovaLozaRepozitorijum vinovaLozaRepozitorijum = new VinoveLozeRepozitorijum(bazaPodataka);

<<<<<<< HEAD
=======
            
>>>>>>> 7d356f703eacf9bf164455f1f8b479e0d8615f72
            IVinskiPodrumRepozitorijum vinskiPodrumRepozitorijum = new VinskiPodrumRepozitorijum(bazaPodataka);

            IFermentacijaRepozitorijum fermentacijaRepozitorijum = new FermentacijaRepozitorijum(bazaPodataka);
            IMerenjeSeceraRepozitorijum merenjeSeceraRepozitorijum = new MerenjeSeceraRepozitorijum(bazaPodataka);
            IEvidencijaProizvodnjeVinaRepozitorijum evidencijaVinaRepozitorijum = new EvidencijaProizvodnjeVinaRepozitorijum(bazaPodataka);

<<<<<<< HEAD
=======
           
>>>>>>> 7d356f703eacf9bf164455f1f8b479e0d8615f72
            IPaleteRepozitorijum paleteRepozitorijum = new PaleteRepozitorijum(bazaPodataka);
            IFaktureRepozitorijum faktureRepozitorijum = new FaktureRepozitorijum(bazaPodataka);

            PocetniPodaci.UbaciInicijalnePodatke(
                korisniciRepozitorijum,
                vinovaLozaRepozitorijum,
                vinoRepozitorijum,
                paleteRepozitorijum,
                faktureRepozitorijum,
                vinskiPodrumRepozitorijum
            );
            bazaPodataka.SacuvajPromene();

            IFermentacijaServis fermentacijaServis = new FermentacijaServis(fermentacijaRepozitorijum);
            IMerenjeSeceraServis merenjeSeceraServis = new MerenjeSeceraServis(merenjeSeceraRepozitorijum, fermentacijaRepozitorijum);
            IEvidencijaProizvodnjeVinaServis evidencijaVinaServis = new EvidencijaProizvodnjeVinaServis(evidencijaVinaRepozitorijum, fermentacijaRepozitorijum);

            IAutentifikacijaServis autentifikacijaServis = new AutentifikacioniServis(korisniciRepozitorijum);

<<<<<<< HEAD
            ILoggerServis loggerServis = new LoggerServis();

            IPaleteServis paleteServis = new PaleteServis(paleteRepozitorijum, vinskiPodrumRepozitorijum, loggerServis);
=======
            IBerbaLozeRepozitorijum berbaLozeRepo = new BerbaLozeRepozitorijum(bazaPodataka);


            
            ILoggerServis loggerServis = new LoggerServis();

            
            IPaleteServis paleteServis = new PaleteServis(paleteRepozitorijum, vinskiPodrumRepozitorijum, loggerServis, evidencijaVinaRepo);

            IBerbaLozeServis berbaLozeServis = new BerbaLozeServis(berbaLozeRepo, loggerServis);

            IProracunGrozdjaServis proracunGrozdjaServis = new ProracunGrozdjaServis();

            IVinovaLozaServis vinovaLozaServis = new VinovaLozaServis(vinovaLozaRepozitorijum);

>>>>>>> 7d356f703eacf9bf164455f1f8b479e0d8615f72

            PaleteMeni paleteMeni = new PaleteMeni(paleteServis);

            AutentifikacioniMeni am = new AutentifikacioniMeni(autentifikacijaServis);
            Korisnik prijavljen;

            while (am.TryLogin(out prijavljen) == false)
            {
                Console.WriteLine("Pogrešno korisničko ime ili lozinka. Pokušajte ponovo.");
            }

            Console.Clear();
            Console.WriteLine($"Uspešno ste prijavljeni kao: {prijavljen.ImePrezime} ({prijavljen.Uloga})");

<<<<<<< HEAD
            ISkladistenjeServis skladistenjeServis;

            if (prijavljen.Uloga == TipKorisnika.GlavniEnolog)
                skladistenjeServis = new VinskiPodrumSkladistenjeServis(paleteRepozitorijum, loggerServis);
            else
                skladistenjeServis = new LokalniKelarSkladistenjeServis(paleteRepozitorijum, loggerServis);

            IPakovanjeServis pakovanjeServis =
                new PakovanjeServis(vinoRepozitorijum, paleteRepozitorijum, skladistenjeServis, loggerServis);

            PakovanjeMeni pakovanjeMeni = new PakovanjeMeni(pakovanjeServis);

            OpcijeMeni meni = new OpcijeMeni(
                evidencijaVinaServis,
                paleteMeni,
                pakovanjeMeni
            );

            meni.Prikazi();
=======
            //Izbor skladistenja
            ISKladistenjeServis skladistenjeServis = new SkladistenjeServis();
            var skladMeni = new SkladistenjeMeni(skladistenjeServis);
            skladMeni.Prikazi();

            var nacin = skladistenjeServis.PreuzmiNacinSkladistenja();
            
            var vinskiMeni = new VinskiPodrumMeni(vinskiPodrumRepozitorijum, skladistenjeServis);
            var lokalniMeni = new LokalniPodrumMeni(vinskiPodrumRepozitorijum, skladistenjeServis);

            if (nacin == NacinSkladistenja.VinskiPodrum)
            {
                vinskiMeni.Prikazi();
            }
            else if (nacin == NacinSkladistenja.LokalniPodrum)
            {
                lokalniMeni.Prikazi();

            }
                // ✅ IZMIJENJENO: dodali smo paleteMeni kao 4. parametar
                OpcijeMeni meni = new OpcijeMeni(fermentacijaServis, merenjeSeceraServis, evidencijaVinaServis, paleteMeni,berbaLozeServis, proracunGrozdjaServis, vinovaLozaServis);
            meni.PrikaziMeni();
>>>>>>> 7d356f703eacf9bf164455f1f8b479e0d8615f72
        }
    }
}
