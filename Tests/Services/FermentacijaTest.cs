using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Moq;
using NUnit.Framework;
using Services.VinogradServisi;
using System;
using System.Collections.Generic;

namespace Tests.Services.VinogradServisi
{
    [TestFixture]
    public class FermentacijaServisTests
    {
        private Mock<IFermentacijaRepozitorijum> fermentacijaRepo = null!;
        private FermentacijaServis fermentacijaServis = null!;

        [SetUp]
        public void Setup()
        {
            fermentacijaRepo = new Mock<IFermentacijaRepozitorijum>();
            fermentacijaServis = new FermentacijaServis(fermentacijaRepo.Object);
        }

        [Test]
        public void ZapocniFermentaciju()
        {
            Guid berbaId = Guid.NewGuid();

            fermentacijaRepo.Setup(x => x.DodajFermentaciju(It.IsAny<Fermentacija>())).Returns((Fermentacija fermentacija) => fermentacija);

            Fermentacija rezultat = fermentacijaServis.ZapocniFermentaciju(berbaId);

            Assert.That(rezultat, Is.Not.Null);
            Assert.That(rezultat.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(rezultat.BerbaId, Is.EqualTo(berbaId));
            Assert.That(rezultat.Faza, Is.EqualTo(FazaFermentacije.Pokrenuta));
        }

        [Test]
        public void PromeniFazu_FermentacijaNePostojie()
        {
            Guid fermentacijaId = Guid.NewGuid();

            fermentacijaRepo.Setup(x => x.PronadjiFermentacijuPoId(fermentacijaId)).Returns((Fermentacija)null!);

            bool ok = fermentacijaServis.PromeniFazu(fermentacijaId, FazaFermentacije.Zavrsena);

            Assert.That(ok, Is.False);
            fermentacijaRepo.Verify(x => x.AzurirajFermentaciju(It.IsAny<Fermentacija>()), Times.Never);
        }

        [Test]
        public void PromeniFazu()
        {
            Guid fermentacijaId = Guid.NewGuid();
            Fermentacija fermentacija = new Fermentacija
            {
                Id = fermentacijaId,
                BerbaId = Guid.NewGuid(),
                DatumPocetka = DateTime.UtcNow.AddDays(-1),
                Faza = FazaFermentacije.Pokrenuta
            };

            fermentacijaRepo.Setup(x => x.PronadjiFermentacijuPoId(fermentacijaId)).Returns(fermentacija);
            fermentacijaRepo.Setup(x => x.AzurirajFermentaciju(It.IsAny<Fermentacija>())).Returns(true);

            bool ok = fermentacijaServis.PromeniFazu(fermentacijaId, FazaFermentacije.Zavrsena);

            Assert.That(ok, Is.True);
            Assert.That(fermentacija.Faza, Is.EqualTo(FazaFermentacije.Zavrsena));
            Assert.That(fermentacija.DatumZavrsetka, Is.Not.EqualTo(default(DateTime)));

            fermentacijaRepo.Verify(x => x.AzurirajFermentaciju(It.Is<Fermentacija>(fermentacija => fermentacija.Id == fermentacijaId)), Times.Once);
        }

        [Test]
        public void PregledSvihFermentacija()
        {
            List<Fermentacija> fermentacije = new List<Fermentacija>
            {
                new Fermentacija { Id = Guid.NewGuid(), BerbaId = Guid.NewGuid(), Faza = FazaFermentacije.Pokrenuta },
                new Fermentacija { Id = Guid.NewGuid(), BerbaId = Guid.NewGuid(), Faza = FazaFermentacije.Zavrsena }
            };

            fermentacijaRepo.Setup(x => x.SveFermentacije()).Returns(fermentacije);

            var rezultat = fermentacijaServis.PregledSvihFermentacija();

            Assert.That(rezultat, Is.EqualTo(fermentacije));
        }

        [Test]
        public void PregledFermentacije()
        {
            Guid fermentacijaId = Guid.NewGuid();
            Fermentacija fermentacija = new Fermentacija
            {
                Id = fermentacijaId,
                BerbaId = Guid.NewGuid(),
                Faza = FazaFermentacije.Pokrenuta
            };

            fermentacijaRepo.Setup(x => x.PronadjiFermentacijuPoId(fermentacijaId)).Returns(fermentacija);

            Fermentacija rezultat = fermentacijaServis.PregledFermentacije(fermentacijaId);

            Assert.That(rezultat, Is.EqualTo(fermentacija));
        }
    }
}
