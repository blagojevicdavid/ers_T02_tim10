using Domain.Enumeracije;
using Domain.Modeli;
using NUnit.Framework;

namespace Tests.Domain
{
    [TestFixture]
    public class KorisnikTests
    {
        [Test]
        [TestCase("petar123", "lozinka123", "Petar Petrovic", TipKorisnika.GlavniEnolog)]
        [TestCase("marko1", "pass321", "Marko Markovic", TipKorisnika.KelarMajstor)]

        public void TestKonstruktora(string korisnickoIme, string lozinka, string imePrezime, TipKorisnika tipKorisnika)
        {
            Korisnik korisnik = new(korisnickoIme, lozinka, imePrezime, tipKorisnika);
            
            Assert.That(korisnik, Is.Not.Null);
            Assert.That(korisnik.Id, Is.EqualTo(0));
            Assert.That(korisnik.KorisnickoIme, Is.EqualTo(korisnickoIme));
            Assert.That(korisnik.Lozinka, Is.EqualTo(lozinka));
            Assert.That(korisnik.ImePrezime, Is.EqualTo(imePrezime));
            Assert.That(korisnik.Uloga, Is.EqualTo(tipKorisnika));
        }

        [Test]
        public void TestPraznogKonstruktora()
        {
            var korisnik = new Korisnik();

            Assert.That(korisnik.Id, Is.EqualTo(0));
            Assert.That(korisnik.KorisnickoIme, Is.EqualTo(string.Empty));
            Assert.That(korisnik.Lozinka, Is.EqualTo(string.Empty));
            Assert.That(korisnik.ImePrezime, Is.EqualTo(string.Empty));
        }
    }
}
