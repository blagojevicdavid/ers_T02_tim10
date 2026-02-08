using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Moq;
using NUnit.Framework;
using Services.VinogradServisi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Services.VinogradServisi
{
    [TestFixture]
    public class PaleteServisTests
    {
        private Mock<IPaleteRepozitorijum> paleteRepozitorijum = null!;
        private Mock<IVinskiPodrumRepozitorijum> vinskiPodrumRepozitorijum = null!;
        private Mock<IEvidencijaProizvodnjeVinaRepozitorijum> evidencijaVinaRepozitorijum = null!;
        private Mock<ILoggerServis> loggerServis = null!;
        private PaleteServis paleteServis = null!;

        [SetUp]
        public void Setup()
        {
            paleteRepozitorijum = new Mock<IPaleteRepozitorijum>();
            vinskiPodrumRepozitorijum = new Mock<IVinskiPodrumRepozitorijum>();
            evidencijaVinaRepozitorijum = new Mock<IEvidencijaProizvodnjeVinaRepozitorijum>();
            loggerServis = new Mock<ILoggerServis>();
            paleteServis = new PaleteServis(paleteRepozitorijum.Object, vinskiPodrumRepozitorijum.Object, loggerServis.Object, evidencijaVinaRepozitorijum.Object);
        }

        [Test]
        public void PosaljiPaleteUVinskiPodrum_NevalidanBrojPaleta()
        {
            var rezultat = paleteServis.PosaljiPaleteUVinskiPodrum(Guid.NewGuid(), 0);

            Assert.That(rezultat, Is.Not.Null);
            Assert.That(rezultat.Count, Is.EqualTo(0));
        }

        [Test]
        public void PosaljiPaleteUVinskiPodrum_PrekoracenLimit()
        {
            var rezultat = paleteServis.PosaljiPaleteUVinskiPodrum(Guid.NewGuid(), 6);

            Assert.That(rezultat, Is.Not.Null);
            Assert.That(rezultat.Count, Is.EqualTo(0));
        }

        [Test]
        public void PregledPalete_PrazanId_VracaNovuUpakovanuPaletu()
        {
            Paleta paleta = paleteServis.PregledPalete(Guid.Empty);

            Assert.That(paleta, Is.Not.Null);
            Assert.That(paleta.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(paleta.Status, Is.EqualTo(StatusPalete.Upakovana));

            paleteRepozitorijum.Verify(x => x.PronadjiPaletuPoId(It.IsAny<Guid>()),Times.Never);
        }


        [Test]
        public void PregledPalete_PrazanId_VracaNovuPaletu()
        {
            Paleta paleta = paleteServis.PregledPalete(Guid.Empty);

            Assert.That(paleta, Is.Not.Null);
            Assert.That(paleta.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(paleta.Status, Is.EqualTo(StatusPalete.Upakovana));

            paleteRepozitorijum.Verify(x => x.PronadjiPaletuPoId(It.IsAny<Guid>()), Times.Never);
        }


        [Test]
        public void DodajProizvedenoVinoNaPaletu_PaletaNePostoji()
        {
            Guid paletaId = Guid.NewGuid();
            Guid evidencijaId = Guid.NewGuid();

            paleteRepozitorijum.Setup(x => x.PronadjiPaletuPoId(paletaId)).Returns(new Paleta());

            bool ok = paleteServis.DodajProizvedenoVinoNaPaletu(paletaId, evidencijaId);

            Assert.That(ok, Is.False);
        }

        [Test]
        public void DodajProizvedenoVinoNaPaletu_EvidencijaNePostoji()
        {
            Guid paletaId = Guid.NewGuid();
            Guid evidencijaId = Guid.NewGuid();

            Paleta paleta = new Paleta{Id = paletaId, VinaIds = new List<Guid>()};

            paleteRepozitorijum.Setup(x => x.PronadjiPaletuPoId(paletaId)).Returns(paleta);
            evidencijaVinaRepozitorijum.Setup(x => x.SveEvidencije()).Returns(new List<EvidencijaProizvodnjeVina>());

            bool ok = paleteServis.DodajProizvedenoVinoNaPaletu(paletaId, evidencijaId);

            Assert.That(ok, Is.False);
        }

        [Test]
        public void DodajProizvedenoVinoNaPaletu_UspesnoDodaje()
        {
            Guid paletaId = Guid.NewGuid();
            Guid evidencijaId = Guid.NewGuid();

            Paleta paleta = new Paleta{Id = paletaId, VinaIds = new List<Guid>()};

            EvidencijaProizvodnjeVina evidencija = new EvidencijaProizvodnjeVina{Id = evidencijaId};

            paleteRepozitorijum.Setup(x => x.PronadjiPaletuPoId(paletaId)).Returns(paleta);
            evidencijaVinaRepozitorijum.Setup(x => x.SveEvidencije()).Returns(new List<EvidencijaProizvodnjeVina> { evidencija });
            paleteRepozitorijum.Setup(x => x.AzurirajPaletu(It.IsAny<Paleta>())).Returns(true);

            bool ok = paleteServis.DodajProizvedenoVinoNaPaletu(paletaId, evidencijaId);

            Assert.That(ok, Is.True);
            Assert.That(paleta.VinaIds.Contains(evidencijaId), Is.True);
        }

        [Test]
        public void DodajProizvedenoVinoNaPaletu_Duplikat()
        {
            Guid paletaId = Guid.NewGuid();
            Guid evidencijaId = Guid.NewGuid();

            Paleta paleta = new Paleta{Id = paletaId, VinaIds = new List<Guid> { evidencijaId }};

            EvidencijaProizvodnjeVina evidencija = new EvidencijaProizvodnjeVina{Id = evidencijaId};

            paleteRepozitorijum.Setup(x => x.PronadjiPaletuPoId(paletaId)).Returns(paleta);
            evidencijaVinaRepozitorijum.Setup(x => x.SveEvidencije()).Returns(new List<EvidencijaProizvodnjeVina> { evidencija });

            bool ok = paleteServis.DodajProizvedenoVinoNaPaletu(paletaId, evidencijaId);

            Assert.That(ok, Is.False);
        }
    }
}
