using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;

namespace Presentation.Authentifikacija
{
    public class AutentifikacioniMeni
    {
        private readonly IAutentifikacijaServis autentifikacijaServis;

        public AutentifikacioniMeni(IAutentifikacijaServis autentifikacijaServis)
        {
            this.autentifikacijaServis = autentifikacijaServis;
        }

        public bool TryLogin(out Korisnik prijavljeni)
        {
            prijavljeni = new Korisnik();

            while (true)
            {
                Console.WriteLine("===== AUTENTIFIKACIJA =====");
                Console.WriteLine("1) Prijava");
                Console.WriteLine("2) Registracija");
                Console.WriteLine("0) Izlaz");
                Console.Write("Izbor: ");

                var izbor = Console.ReadLine();
                if (izbor == null)
                {
                    izbor = "";
                }

                if (izbor == null)
                {
                    izbor = "";
                }


                switch (izbor)
                {
                    case "1":
                        if (Prijava(out prijavljeni))
                            return true;
                        break;

                    case "2":
                        Registracija();
                        break;

                    case "0":
                        return false;

                    default:
                        Console.WriteLine("Nepoznata opcija.\n");
                        break;
                }
            }
        }

        private bool Prijava(out Korisnik korisnik)
        {
            korisnik = new Korisnik();

            Console.Write("Korisničko ime: ");
            var korisnickoIme = Console.ReadLine();
            if (korisnickoIme == null)
            {
                korisnickoIme = "";
            }

            Console.Write("Lozinka: ");
            var lozinka = Console.ReadLine();
            if (lozinka == null)
            {
                lozinka = "";
            }

            var (uspesno, prijavljeni) =
                autentifikacijaServis.Prijava(korisnickoIme.Trim(), lozinka.Trim());

            if (!uspesno)
            {
                Console.WriteLine("Neuspešna prijava.\n");
                return false;
            }

            korisnik = prijavljeni;
            return true;
        }

        private void Registracija()
        {
            Console.WriteLine("\n=== REGISTRACIJA ===");

            Korisnik novi = new Korisnik();

            Console.Write("Ime i prezime: ");
            var imePrezime = Console.ReadLine();
            if (imePrezime == null)
            {
                imePrezime = "";
            }
            novi.ImePrezime = imePrezime;

            Console.Write("Korisničko ime: ");
            var korisnickoIme = Console.ReadLine();
            if (korisnickoIme == null)
            {
                korisnickoIme = "";
            }
            novi.KorisnickoIme = korisnickoIme;

            Console.Write("Lozinka: ");
            var lozinka = Console.ReadLine();
            if (lozinka == null)
            {
                lozinka = "";
            }
            novi.Lozinka = lozinka;

            Console.WriteLine("Izaberite ulogu:");
            Console.WriteLine("1) Glavni enolog");
            Console.WriteLine("2) Kelar majstor");
            Console.Write("Izbor: ");

            var izborUloge = Console.ReadLine();
            if (izborUloge == null)
            {
                izborUloge = "";
            }

            if (izborUloge == "1")
                novi.Uloga = TipKorisnika.GlavniEnolog;
            else if (izborUloge == "2")
                novi.Uloga = TipKorisnika.KelarMajstor;
            else
            {
                Console.WriteLine("Pogrešan izbor uloge.\n");
                return;
            }

            var (uspesno, dodat) = autentifikacijaServis.Registracija(novi);

            if (uspesno)
                Console.WriteLine("Registracija uspešna.\n");
            else
                Console.WriteLine("Registracija neuspešna.\n");
        }
    }
}
