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

            IVinskiPodrumRepozitorijum vinskiPodrumRepozitorijum = new VinskiPodrumRepozitorijum(bazaPodataka);

            IFermentacijaRepozitorijum fermentacijaRepozitorijum = new FermentacijaRepozitorijum(bazaPodataka);
            IMerenjeSeceraRepozitorijum merenjeSeceraRepozitorijum = new MerenjeSeceraRepozitorijum(bazaPodataka);
            IEvidencijaProizvodnjeVinaRepozitorijum evidencijaVinaRepozitorijum = new EvidencijaProizvodnjeVinaRepozitorijum(bazaPodataka);

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

            ILoggerServis loggerServis = new LoggerServis();

            IPaleteServis paleteServis = new PaleteServis(paleteRepozitorijum, vinskiPodrumRepozitorijum, loggerServis);

            PaleteMeni paleteMeni = new PaleteMeni(paleteServis);

            AutentifikacioniMeni am = new AutentifikacioniMeni(autentifikacijaServis);
            Korisnik prijavljen;

            while (am.TryLogin(out prijavljen) == false)
            {
                Console.WriteLine("Pogrešno korisničko ime ili lozinka. Pokušajte ponovo.");
            }

            Console.Clear();
            Console.WriteLine($"Uspešno ste prijavljeni kao: {prijavljen.ImePrezime} ({prijavljen.Uloga})");

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
        }
    }
}
