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
using Services.FaktureServisi;
using Services.IsporukaServis;
using Services.LoggerServisi;
using Services.PakovanjeServisi;
using Services.ProdajaServisi;
using Services.SkladistenjeServisi;
using Services.VinogradServisi;
using Services.VinoServisi;

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

            IAutentifikacijaServis autentifikacijaServis =
                new AutentifikacioniServis(korisniciRepozitorijum, loggerServis);

            IFermentacijaServis fermentacijaServis = new FermentacijaServis(fermentacijaRepozitorijum);

            IMerenjeSeceraServis merenjeSeceraServis =
                new MerenjeSeceraServis(merenjeSeceraRepozitorijum, fermentacijaRepozitorijum);

            IEvidencijaProizvodnjeVinaServis evidencijaVinaServis =
                new EvidencijaProizvodnjeVinaServis(evidencijaVinaRepozitorijum, fermentacijaRepozitorijum);

            IPaleteServis paleteServis = new PaleteServis(
                paleteRepozitorijum,
                vinskiPodrumRepozitorijum,
                loggerServis,
                evidencijaVinaRepozitorijum
            );


            IBerbaLozeServis berbaLozeServis = new BerbaLozeServis(berbaLozeRepozitorijum, loggerServis);
            IProracunGrozdjaServis proracunGrozdjaServis = new ProracunGrozdjaServis();
            IVinovaLozaServis vinovaLozaServis = new VinovaLozaServis(vinovaLozaRepozitorijum);
            IProizvodnjaVinaServis proizvodnjaVinaServis = new ProizvodnjaVinaServis(
    vinovaLozaRepozitorijum,
    vinoRepozitorijum,
    vinovaLozaServis,
    berbaLozeServis,
    fermentacijaServis,
    merenjeSeceraServis,
    evidencijaVinaServis,
    loggerServis
);


            IProdajaServis prodajaServis = new ProdajaServis(
                paleteRepozitorijum,
                faktureRepozitorijum,
                loggerServis
            );

            IFakturePregledServis fakturePregledServis = new FakturePregledServis(faktureRepozitorijum);

            IPonudaVinaServis ponudaVinaServis = new PonudaVinaServis(vinoRepozitorijum);
            IOdabirKolicineVinaServis odabirKolicineVinaServis = new OdabirKolicineVinaServis(ponudaVinaServis);

            // -------------------- LOGIN --------------------
            AutentifikacioniMeni am = new AutentifikacioniMeni(autentifikacijaServis);
            Korisnik prijavljen;

            while (!am.TryLogin(out prijavljen))
            {
                Console.WriteLine("Pogrešno korisničko ime ili lozinka. Pokušajte ponovo.");
            }

            Console.Clear();
            Console.WriteLine($"Uspešno ste prijavljeni kao: {prijavljen.ImePrezime} ({prijavljen.Uloga})");

            // -------------------- SKLADISTENJE  --------------------
            ISkladistenjeServis skladistenjeServis;

            if (prijavljen.Uloga == TipKorisnika.GlavniEnolog)
                skladistenjeServis = new VinskiPodrumSkladistenjeServis(paleteRepozitorijum, loggerServis);
            else
                skladistenjeServis = new LokalniKelarSkladistenjeServis(paleteRepozitorijum, loggerServis);

            IPakovanjeServis pakovanjeServis =
                new PakovanjeServis(vinoRepozitorijum, paleteRepozitorijum, skladistenjeServis, proizvodnjaVinaServis, loggerServis);


            IIsporukaVinaServis isporukaVinaServis =
                new IsporukaVinaServis(skladistenjeServis, loggerServis);





            IProdajaTokServis prodajaTokServis = new ProdajaTokServis(pakovanjeServis, skladistenjeServis, prodajaServis, loggerServis, vinoRepozitorijum, paleteRepozitorijum);









            // -------------------- MENIJI --------------------
            // Prodaja / pregled (enolog)
            PonudaVinaMeni ponudaVinaMeni = new PonudaVinaMeni(ponudaVinaServis);
            OdabirKolicineVinaMeni odabirKolicineVinaMeni = new OdabirKolicineVinaMeni(ponudaVinaServis, odabirKolicineVinaServis);
            ProdajaMeni prodajaMeni = new ProdajaMeni(prodajaTokServis, skladistenjeServis, vinskiPodrumRepozitorijum);



            FaktureMeni faktureMeni = new FaktureMeni(fakturePregledServis);

            // Vinograd/berba (enolog)
            VinovaLozaMeni vinovaLozaMeni = new VinovaLozaMeni(vinovaLozaServis);
            BerbaLozeMeni berbaLozeMeni = new BerbaLozeMeni(berbaLozeServis);
            ProracunGrozdjaMeni proracunGrozdjaMeni = new ProracunGrozdjaMeni(proracunGrozdjaServis);

            // Operativa (kelar)
            PakovanjeMeni pakovanjeMeni = new PakovanjeMeni(pakovanjeServis);
            PaleteMeni paleteMeni = new PaleteMeni(paleteServis);
            SkladistenjeMeni skladistenjeMeni = new SkladistenjeMeni(skladistenjeServis);
            IsporukaVinaMeni isporukaVinaMeni = new IsporukaVinaMeni(isporukaVinaServis);

            FermentacijaMeni fermentacijaMeni = new FermentacijaMeni(fermentacijaServis, merenjeSeceraServis);
            ProizvodnjaVinaMeni proizvodnjaVinaMeni = new ProizvodnjaVinaMeni(evidencijaVinaServis, paleteMeni, pakovanjeMeni);

            // -------------------- GLAVNI MENI --------------------
            OpcijeMeni meni = new OpcijeMeni(
                ponudaVinaMeni,
                odabirKolicineVinaMeni,
                prodajaMeni,
                faktureMeni,
                vinovaLozaMeni,
                berbaLozeMeni,
                proracunGrozdjaMeni,
                fermentacijaMeni,
                proizvodnjaVinaMeni,
                pakovanjeMeni,
                paleteMeni,
                skladistenjeMeni,
                isporukaVinaMeni
            );

            if (prijavljen.Uloga == TipKorisnika.GlavniEnolog)
                meni.PrikaziEnolog();
            else if (prijavljen.Uloga == TipKorisnika.KelarMajstor)
                meni.PrikaziKelarMajstor();
        }
    }
}
