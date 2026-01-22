using Database.BazaPodataka;
using Database.BazaPodataka.Database.BazaPodataka;
using Database.Repozitorijumi;
using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Presentation.Authentifikacija;
using Presentation.Meni;
using Services.AutentifikacioniServisi;
using Services.VinogradServisi;



namespace Loger_Bloger
{
    public class Program
    {
        public static void Main()
        {
            // Baza podataka
            IBazaPodataka bazaPodataka = new XmlBazaPodataka();

            // Repozitorijumi
            IKorisniciRepozitorijum korisniciRepozitorijum = new KorisniciRepozitorijum(bazaPodataka);

            IVinoRepozitorijum vinoRepozitorijum = new VinoRepozitorijum(bazaPodataka);

            IVinovaLozaRepozitorijum vinovaLozaRepozitorijum = new VinoveLozeRepozitorijum(bazaPodataka);

            IVinskiPodrumRepozitorijum CinskipodrumRepo = new VinskiPodrumRepozitorijum(bazaPodataka);

            IFermentacijaRepozitorijum fermentacijaRepozitorijum = new FermentacijaRepozitorijum(bazaPodataka);

            IFermentacijaServis fermentacijaServis = new FermentacijaServis(fermentacijaRepozitorijum);

            IMerenjeSeceraRepozitorijum merenjeSeceraRepozitorijum = new MerenjeSeceraRepozitorijum(bazaPodataka);

            IMerenjeSeceraServis merenjeSeceraServis = new MerenjeSeceraServis(merenjeSeceraRepozitorijum, fermentacijaRepozitorijum);



            // Servisi
            IAutentifikacijaServis autentifikacijaServis =
    new AutentifikacioniServis(korisniciRepozitorijum);
            // TODO: Pass necessary dependencies
            // TODO: Add other necessary services

            // Ako nema nijednog korisnika u sistemu, dodati dva nova
            if (korisniciRepozitorijum.SviKorisnici().Count() == 0)
            {
                // TODO: Add initial users to the system
            }

            // Prezentacioni sloj
            AutentifikacioniMeni am = new AutentifikacioniMeni(autentifikacijaServis);
            Korisnik prijavljen = new Korisnik();

            while (am.TryLogin(out prijavljen) == false)
            {
                Console.WriteLine("Pogrešno korisničko ime ili lozinka. Pokušajte ponovo.");
            }

            Console.Clear();
            Console.WriteLine($"Uspešno ste prijavljeni kao: {prijavljen.ImePrezime} ({prijavljen.Uloga})");

            OpcijeMeni meni = new OpcijeMeni(fermentacijaServis, merenjeSeceraServis); // TODO: Pass necessary dependencies
            meni.PrikaziMeni();


        }
    }
}
