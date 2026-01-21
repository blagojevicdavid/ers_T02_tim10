using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IAutentifikacijaServis
    {
        (bool, Korisnik) Prijava(string korisnickoIme, string lozinka);
        (bool, Korisnik) Registracija(Korisnik noviKorisnik);
    }
}
