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

                string izbor = Console.ReadLine() ?? "";

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
            string korisnickoIme = Console.ReadLine() ?? "";

            Console.Write("Lozinka: ");
            string lozinka = Console.ReadLine() ?? "";

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
            novi.ImePrezime = Console.ReadLine() ?? "";

            Console.Write("Korisničko ime: ");
            novi.KorisnickoIme = Console.ReadLine() ?? "";

            Console.Write("Lozinka: ");
            novi.Lozinka = Console.ReadLine() ?? "";

            // IZBOR ULOGE (ENUM – NEMA string greške)
            Console.WriteLine("Izaberite ulogu:");
            Console.WriteLine("1) Glavni enolog");
            Console.WriteLine("2) Kelar majstor");
            Console.Write("Izbor: ");

            string izborUloge = Console.ReadLine() ?? "";

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
