using System;
using System.Linq;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.ProdajaServisi
{
    public class ProdajaServis : IProdajaServis
    {
        private readonly IPaleteRepozitorijum paleteRepozitorijum;
        private readonly IFaktureRepozitorijum faktureRepozitorijum;
        private readonly ILoggerServis loggerServis;

        public ProdajaServis(
            IPaleteRepozitorijum paleteRepozitorijum,
            IFaktureRepozitorijum faktureRepozitorijum,
            ILoggerServis loggerServis)
        {
            this.paleteRepozitorijum = paleteRepozitorijum ?? throw new ArgumentNullException(nameof(paleteRepozitorijum));
            this.faktureRepozitorijum = faktureRepozitorijum ?? throw new ArgumentNullException(nameof(faktureRepozitorijum));
            this.loggerServis = loggerServis ?? throw new ArgumentNullException(nameof(loggerServis));
        }

        public Guid IsporuciVinoKupcu(
            Guid paletaId,
            string kupac,
            decimal cenaPoKomadu,
            TipProdaje tipProdaje,
            NacinPlacanja nacinPlacanja)
        {
            if (paletaId == Guid.Empty)
                throw new ArgumentException("PaletaId je obavezan.");

            if (string.IsNullOrWhiteSpace(kupac))
                throw new ArgumentException("Kupac je obavezan.");

            if (cenaPoKomadu <= 0)
                throw new ArgumentException("Cena po komadu mora biti > 0.");

            Paleta paleta = paleteRepozitorijum.PronadjiPaletuPoId(paletaId);
            if (paleta == null || paleta.Id == Guid.Empty)
                throw new InvalidOperationException("Paleta ne postoji.");

            if (paleta.Status != StatusPalete.Otpremljena && paleta.Status != StatusPalete.Raspakovana)
            {
                loggerServis.Evidentiraj(
                    TipEvidencije.ERROR,
                    $"Pokušaj prodaje palete koja nije spremna. Paleta={paleta.Sifra}, Status={paleta.Status}"
                );
                throw new InvalidOperationException("Paleta nije spremna za prodaju (mora biti Otpremljena ili Raspakovana).");
            }

            if (paleta.VinaIds == null || paleta.VinaIds.Count == 0)
                throw new InvalidOperationException("Paleta nema vina.");

            var stavke = paleta.VinaIds
                .GroupBy(id => id)
                .Select(g => new StavkaFakture(g.Key, g.Count(), cenaPoKomadu))
                .ToList();

            Faktura faktura = new Faktura
            {
                TipProdaje = tipProdaje,
                NacinPlacanja = nacinPlacanja,
                Stavke = stavke
            };

            faktureRepozitorijum.DodajFakturu(faktura);

            paleta.Status = StatusPalete.Isporucena;
            paleteRepozitorijum.AzurirajPaletu(paleta);

            loggerServis.Evidentiraj(
                TipEvidencije.INFO,
                $"Isporucena paleta {paleta.Sifra} kupcu {kupac}. Faktura={faktura.Id}, TipProdaje={tipProdaje}, Placanje={nacinPlacanja}"
            );

            return faktura.Id;
        }
    }
}
