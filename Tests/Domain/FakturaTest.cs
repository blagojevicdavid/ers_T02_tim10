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
    public class FakturaTests
    {
        [Test]
        public void KonstruktorBezParametara()
        {
            Faktura faktura = new Faktura();

            Assert.That(faktura, Is.Not.Null);
            Assert.That(faktura.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(faktura.DatumIzdavanja, Is.Not.EqualTo(default(DateTime)));
            Assert.That(faktura.Stavke, Is.Not.Null);
            Assert.That(faktura.Stavke.Count, Is.EqualTo(0));
        }

        [Test]
        [TestCase(TipProdaje.Diskont, NacinPlacanja.Gotovina)]
        [TestCase(TipProdaje.Restoranska, NacinPlacanja.GotovinskiRacun)]
        public void KonstruktorSaParametrima(TipProdaje tipProdaje, NacinPlacanja nacinPlacanja)
        {
            Guid id = Guid.NewGuid();
            DateTime datum = new DateTime(2025, 3, 10);
            List<StavkaFakture> stavke = new List<StavkaFakture>();

            Faktura faktura = new Faktura(id, datum, tipProdaje, nacinPlacanja, stavke);

            Assert.That(faktura.Id, Is.EqualTo(id));
            Assert.That(faktura.DatumIzdavanja, Is.EqualTo(datum));
            Assert.That(faktura.TipProdaje, Is.EqualTo(tipProdaje));
            Assert.That(faktura.NacinPlacanja, Is.EqualTo(nacinPlacanja));
            Assert.That(faktura.Stavke, Is.EqualTo(stavke));
            Assert.That(faktura.Stavke.Count, Is.EqualTo(0));
        }

        [Test]
        public void Dodavanje()
        {
            Faktura faktura = new Faktura();
            StavkaFakture stavka = new StavkaFakture();

            faktura.Stavke.Add(stavka);

            Assert.That(faktura.Stavke.Count, Is.EqualTo(1));
        }
    }
}