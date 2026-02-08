using Domain.Enumeracije;
using Domain.Modeli;
using NUnit.Framework;
using System;

namespace Tests.Domain
{
    [TestFixture]
    public class VinoTests
    {
        [Test]
        [TestCase("Merlot", KategorijaVina.Stolno, 0.75, "MER-2025-001")]
        [TestCase("Vranac", KategorijaVina.Kvalitetno, 1.0, "VRA-2025-010")]
        [TestCase("Probno vino", KategorijaVina.Premium, 1.0, "PRB-2025-999")]

        public void KonstruktorOkej(string naziv, KategorijaVina kategorija, double zapreminaLitara, string sifra)
        {
            Guid id = Guid.NewGuid();
            Guid vinovaLozaId = Guid.NewGuid();
            DateTime datumFlasiranja = new DateTime(2025, 3, 10);

            Vino vino = new Vino(id, naziv, kategorija, zapreminaLitara, sifra, vinovaLozaId, datumFlasiranja);

            Assert.That(vino, Is.Not.Null);
            Assert.That(vino.Id, Is.EqualTo(id));
            Assert.That(vino.Naziv, Is.EqualTo(naziv));
            Assert.That(vino.Kategorija, Is.EqualTo(kategorija));
            Assert.That(vino.ZapreminaLitara, Is.EqualTo(zapreminaLitara));
            Assert.That(vino.Sifra, Is.EqualTo(sifra));
            Assert.That(vino.VinovaLozaId, Is.EqualTo(vinovaLozaId));
            Assert.That(vino.DatumFlasiranja, Is.EqualTo(datumFlasiranja));
        }
    }
}
