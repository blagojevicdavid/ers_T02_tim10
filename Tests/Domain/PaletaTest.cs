using Domain.Enumeracije;
using Domain.Modeli;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Domain
{
    [TestFixture]
    public class PaletaTests
    {
        [Test]
        public void TestPraznogKonstruktora()
        {
            Paleta paleta = new Paleta();

            Assert.That(paleta, Is.Not.Null);
            Assert.That(paleta.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(paleta.Sifra, Is.EqualTo(string.Empty));
            Assert.That(paleta.AdresaOdredista, Is.EqualTo(string.Empty));
            Assert.That(paleta.VinskiPodrumId, Is.EqualTo(Guid.Empty));
            Assert.That(paleta.VinaIds, Is.Not.Null);
            Assert.That(paleta.VinaIds.Count, Is.EqualTo(0));
            Assert.That(paleta.Status, Is.EqualTo(StatusPalete.Upakovana));
        }

        [Test]
        [TestCase("PAL001", "Novi Sad", StatusPalete.Upakovana)]
        [TestCase("PAL002", "Beograd", StatusPalete.Raspakovana)]
        [TestCase("PAL003", "Nis", StatusPalete.Isporucena)]
        public void TestKonstruktora(string sifra, string adresa, StatusPalete status)
        {
            Guid id = Guid.NewGuid();
            Guid vinskiPodrumId = Guid.NewGuid();
            List<Guid> vinaIds = new() { Guid.NewGuid(), Guid.NewGuid() };

            Paleta paleta = new Paleta(id, sifra, adresa, vinskiPodrumId, vinaIds, status);

            Assert.That(paleta, Is.Not.Null);
            Assert.That(paleta.Id, Is.EqualTo(id));
            Assert.That(paleta.Sifra, Is.EqualTo(sifra));
            Assert.That(paleta.AdresaOdredista, Is.EqualTo(adresa));
            Assert.That(paleta.VinskiPodrumId, Is.EqualTo(vinskiPodrumId));
            Assert.That(paleta.VinaIds, Is.EqualTo(vinaIds));
            Assert.That(paleta.Status, Is.EqualTo(status));
        }
    }
}
