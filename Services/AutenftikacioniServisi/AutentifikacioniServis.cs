using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Domain.Enumeracije;


namespace Services.AutentifikacioniServisi
{
    public class AutentifikacioniServis : IAutentifikacijaServis
    {
        private readonly IKorisniciRepozitorijum korisniciRepozitorijum;
        private readonly ILoggerServis loggerServis;


        public AutentifikacioniServis(IKorisniciRepozitorijum korisniciRepozitorijum, ILoggerServis loggerServis)
        {
            this.korisniciRepozitorijum = korisniciRepozitorijum;
            this.loggerServis = loggerServis;
        }

        public (bool, Korisnik) Prijava(string korisnickoIme, string lozinka)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(korisnickoIme) || string.IsNullOrWhiteSpace(lozinka))
                {
                    loggerServis.Evidentiraj(
                        TipEvidencije.WARNING,
                        "[AUTH] Prijava neuspesna – prazno korisnicko ime ili lozinka."
                    );
                    return (false, new Korisnik());
                }


                Korisnik korisnik = korisniciRepozitorijum
                    .PronadjiKorisnikaPoKorisnickomImenu(korisnickoIme.Trim());

                // repo vraća prazan korisnik ako ne postoji
                if (string.IsNullOrEmpty(korisnik.KorisnickoIme))
                {
                    loggerServis.Evidentiraj(
                        TipEvidencije.WARNING,
                        $"[AUTH] Prijava neuspesna – korisnik ne postoji ({korisnickoIme})."
                    );
                    return (false, new Korisnik());
                }

                if (korisnik.Lozinka != lozinka.Trim())
                {
                    loggerServis.Evidentiraj(
                        TipEvidencije.WARNING,
                        $"[AUTH] Prijava neuspesna – pogresna lozinka ({korisnickoIme})."
                    );
                    return (false, new Korisnik());
                }

                loggerServis.Evidentiraj(TipEvidencije.INFO,$"[AUTH] Uspesna prijava – {korisnik.ImePrezime} ({korisnik.Uloga})."
);

                return (true, korisnik);
            }
            catch (Exception ex)
            {
                loggerServis.Evidentiraj(
                    TipEvidencije.ERROR,
                    $"[AUTH] Greska tokom prijave: {ex.Message}"
                );
                return (false, new Korisnik());
            }
        }

        public (bool, Korisnik) Registracija(Korisnik noviKorisnik)
        {
            try
            {
                if (noviKorisnik == null)
                {
                    loggerServis.Evidentiraj(
                        TipEvidencije.ERROR,
                        "[AUTH] Neuspesna registracija: noviKorisnik je null."
                    );
                    return (false, new Korisnik());
                }

                if (string.IsNullOrWhiteSpace(noviKorisnik.KorisnickoIme) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.Lozinka) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.ImePrezime))
                {
                    loggerServis.Evidentiraj(
                        TipEvidencije.WARNING,
                        "[AUTH] Neuspesna registracija: nedostaju obavezna polja."
                    );
                    return (false, new Korisnik());
                }

                string ime = noviKorisnik.KorisnickoIme.Trim();

                // provjera da li već postoji
                Korisnik postoji = korisniciRepozitorijum
                    .PronadjiKorisnikaPoKorisnickomImenu(ime);

                if (!string.IsNullOrEmpty(postoji.KorisnickoIme))
                {
                    loggerServis.Evidentiraj(
                        TipEvidencije.WARNING,
                        $"[AUTH] Neuspesna registracija: korisnicko ime zauzeto ({ime})."
                    );
                    return (false, new Korisnik());
                }

                Korisnik dodat = korisniciRepozitorijum.DodajKorisnika(noviKorisnik);

                if (string.IsNullOrEmpty(dodat.KorisnickoIme))
                {
                    loggerServis.Evidentiraj(
                        TipEvidencije.ERROR,
                        $"[AUTH] Neuspesna registracija: repo nije dodao korisnika ({ime})."
                    );
                    return (false, new Korisnik());
                }

                loggerServis.Evidentiraj(
                    TipEvidencije.INFO,
                    $"[AUTH] Uspesna registracija: {dodat.ImePrezime} ({dodat.KorisnickoIme})."
                );

                return (true, dodat);
            }
            catch (Exception ex)
            {
                loggerServis.Evidentiraj(
                    TipEvidencije.ERROR,
                    $"[AUTH] Greska u registraciji: {ex.Message}"
                );
                return (false, new Korisnik());
            }
        }

    }
}
