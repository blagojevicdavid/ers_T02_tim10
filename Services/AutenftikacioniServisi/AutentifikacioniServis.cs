using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.AutentifikacioniServisi
{
    public class AutentifikacioniServis : IAutentifikacijaServis
    {
        private readonly IKorisniciRepozitorijum korisniciRepozitorijum;

        public AutentifikacioniServis(IKorisniciRepozitorijum korisniciRepozitorijum)
        {
            this.korisniciRepozitorijum = korisniciRepozitorijum;
        }

        public (bool, Korisnik) Prijava(string korisnickoIme, string lozinka)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(korisnickoIme) || string.IsNullOrWhiteSpace(lozinka))
                    return (false, new Korisnik());

                Korisnik korisnik = korisniciRepozitorijum
                    .PronadjiKorisnikaPoKorisnickomImenu(korisnickoIme.Trim());

                // repo vraća prazan korisnik ako ne postoji
                if (string.IsNullOrEmpty(korisnik.KorisnickoIme))
                    return (false, new Korisnik());

                if (korisnik.Lozinka != lozinka.Trim())
                    return (false, new Korisnik());

                return (true, korisnik);
            }
            catch
            {
                return (false, new Korisnik());
            }
        }

        public (bool, Korisnik) Registracija(Korisnik noviKorisnik)
        {
            try
            {
                if (noviKorisnik == null)
                    return (false, new Korisnik());

                if (string.IsNullOrWhiteSpace(noviKorisnik.KorisnickoIme) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.Lozinka) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.ImePrezime))
                    return (false, new Korisnik());

                // provjera da li već postoji
                Korisnik postoji = korisniciRepozitorijum
                    .PronadjiKorisnikaPoKorisnickomImenu(noviKorisnik.KorisnickoIme.Trim());

                if (!string.IsNullOrEmpty(postoji.KorisnickoIme))
                    return (false, new Korisnik());

                Korisnik dodat = korisniciRepozitorijum.DodajKorisnika(noviKorisnik);

                if (string.IsNullOrEmpty(dodat.KorisnickoIme))
                    return (false, new Korisnik());

                return (true, dodat);
            }
            catch
            {
                return (false, new Korisnik());
            }
        }
    }
}
