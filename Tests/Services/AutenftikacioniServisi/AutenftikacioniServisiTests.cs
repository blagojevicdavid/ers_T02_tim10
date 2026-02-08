using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Moq;
using NUnit.Framework;
using Services.AutentifikacioniServisi;

namespace Tests.Services.AutentifikacioniServisi
{
    [TestFixture]
    public class AutentifikacioniServisTests
    {
        private Mock<IKorisniciRepozitorijum> _korisniciRepo = null!;
        private Mock<ILoggerServis> _logger = null!;
        private AutentifikacioniServis _servis = null!;

        [SetUp]
        public void Setup()
        {
            _korisniciRepo = new Mock<IKorisniciRepozitorijum>();
            _logger = new Mock<ILoggerServis>();
            _servis = new AutentifikacioniServis(_korisniciRepo.Object, _logger.Object);
        }

        [Test]
        [TestCase("", "pass")]
        [TestCase("user", "")]
        [TestCase("   ", "pass")]
        [TestCase("user", "   ")]
        public void Prijava_PrazanUlaz(string korisnickoIme, string lozinka)
        {
            var (ok, korisnik) = _servis.Prijava(korisnickoIme, lozinka);

            Assert.That(ok, Is.False);
            Assert.That(korisnik.KorisnickoIme, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Prijava_KorisnikNePostoji()
        {
            _korisniciRepo.Setup(x => x.PronadjiKorisnikaPoKorisnickomImenu("pero")).Returns(new Korisnik());

            var (ok, korisnik) = _servis.Prijava("pero", "123");

            Assert.That(ok, Is.False);
            Assert.That(korisnik.KorisnickoIme, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Prijava_PogresnaLozinka()
        {
            var postojeci = new Korisnik("pero", "pass123", "Pera Peric", TipKorisnika.GlavniEnolog);

            _korisniciRepo.Setup(x => x.PronadjiKorisnikaPoKorisnickomImenu("pera")).Returns(postojeci);
            var (ok, korisnik) = _servis.Prijava("pera", "pogresna");

            Assert.That(ok, Is.False);
            Assert.That(korisnik.KorisnickoIme, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Prijava_Uspesna()
        {
            var postojeci = new Korisnik("pero", "123", "Pero Peric", TipKorisnika.KelarMajstor);

            _korisniciRepo.Setup(x => x.PronadjiKorisnikaPoKorisnickomImenu("pero")).Returns(postojeci);

            var (ok, korisnik) = _servis.Prijava("pero", "123");

            Assert.That(ok, Is.True);
            Assert.That(korisnik.KorisnickoIme, Is.EqualTo("pero"));
            Assert.That(korisnik.Uloga, Is.EqualTo(TipKorisnika.KelarMajstor));
        }

        [Test]
        public void Registracija_Null()
        {
            var (ok, korisnik) = _servis.Registracija(null!);

            Assert.That(ok, Is.False);
            Assert.That(korisnik.KorisnickoIme, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Registracija_NedostajuPolja()
        {
            var k1 = new Korisnik("", "123", "Pero Peric", TipKorisnika.GlavniEnolog);
            var k2 = new Korisnik("pera", "", "Pero Peric", TipKorisnika.GlavniEnolog);
            var k3 = new Korisnik("pero", "123", "", TipKorisnika.GlavniEnolog);

            Assert.That(_servis.Registracija(k1).Item1, Is.False);
            Assert.That(_servis.Registracija(k2).Item1, Is.False);
            Assert.That(_servis.Registracija(k3).Item1, Is.False);
        }

        [Test]
        public void Registracija_KorisnickoImeZauzeto()
        {
            var novi = new Korisnik("pero", "123", "Pero Peric", TipKorisnika.GlavniEnolog);
            var postojeci = new Korisnik("pero", "xxx", "Simo Vidić", TipKorisnika.KelarMajstor);

            _korisniciRepo.Setup(x => x.PronadjiKorisnikaPoKorisnickomImenu("pera")).Returns(postojeci);

            var (ok, korisnik) = _servis.Registracija(novi);

            Assert.That(ok, Is.False);
            Assert.That(korisnik.KorisnickoIme, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Registracija_Uspesna()
        {
            var novi = new Korisnik("pero", "123", "Pero Peric", TipKorisnika.KelarMajstor);

            _korisniciRepo.Setup(x => x.PronadjiKorisnikaPoKorisnickomImenu("pero")).Returns(new Korisnik());

            _korisniciRepo.Setup(x => x.DodajKorisnika(novi)).Returns(novi);

            var (ok, korisnik) = _servis.Registracija(novi);

            Assert.That(ok, Is.True);
            Assert.That(korisnik.KorisnickoIme, Is.EqualTo("pero"));
        }
    }
}
