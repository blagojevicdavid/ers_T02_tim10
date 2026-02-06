using System;
using System.Linq;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;

namespace Services.ProdajaServisi
{
    public class ProdajaTokServis : IProdajaTokServis
    {
        private readonly IPakovanjeServis _pakovanje;
        private readonly ISkladistenjeServis _skladistenje;
        private readonly IProdajaServis _prodaja;
        private readonly ILoggerServis _logger;

        public ProdajaTokServis(
            IPakovanjeServis pakovanje,
            ISkladistenjeServis skladistenje,
            IProdajaServis prodaja,
            ILoggerServis logger)
        {
            _pakovanje = pakovanje ?? throw new ArgumentNullException(nameof(pakovanje));
            _skladistenje = skladistenje ?? throw new ArgumentNullException(nameof(skladistenje));
            _prodaja = prodaja ?? throw new ArgumentNullException(nameof(prodaja));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Guid IzvrsiProdaju(
            string nazivVina,
            KategorijaVina kategorija,
            int brojFlasa,
            double zapremina,
            TipProdaje tipProdaje,
            NacinPlacanja nacinPlacanja,
            string adresaOdredista,
            Guid vinskiPodrumId,
            string kupac)
        {
            // 1) Napravi ili pronađi paletu i pošalji je u skladište (status Otpremljena)
            var (ok, paleta) = _pakovanje.PosaljiPrvuDostupnuUpakovanuPaletu(
                nazivVina, kategorija, brojFlasa, zapremina, adresaOdredista, vinskiPodrumId);

            if (!ok || paleta == null || paleta.Id == Guid.Empty)
                throw new InvalidOperationException("Neuspešno pakovanje/otprema palete.");

            // 2) Skladište priprema palete za prodaju (u tvom kodu menja status u Raspakovana).
            // BITNO: ne uzimamo "prvu iz liste" da ne prodamo pogrešnu paletu.
            _skladistenje.IsporuciPaleteZaProdaju(1).ToList();

            // 3) Cena (minimalno pravilo)
            decimal cenaPoKomadu = IzracunajCenu(tipProdaje);

            // 4) Prodaj baš paletu koju smo upravo poslali
            Guid fakturaId = _prodaja.IsporuciVinoKupcu(paleta.Id, kupac, cenaPoKomadu, tipProdaje, nacinPlacanja);


            _logger.Evidentiraj(TipEvidencije.INFO,
                $"[PRODAJA TOK] Prodaja OK. Paleta={paleta.Sifra}, Faktura={fakturaId}, Placanje={nacinPlacanja}");

            return fakturaId;
        }

        private decimal IzracunajCenu(TipProdaje tipProdaje)
        {
            decimal bazna = 10m;

            // OVDJE prilagodi tačan naziv enum vrijednosti koji imaš:
            // ako je kod tebe TipProdaje.DiskontPica, zamijeni TipProdaje.Diskont sa DiskontPica
            if (tipProdaje == TipProdaje.Diskont)
                return bazna * 0.85m;

            return bazna;
        }
    }
}
