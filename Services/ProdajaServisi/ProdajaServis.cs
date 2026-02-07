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
<<<<<<< HEAD
            this.paleteRepozitorijum = paleteRepozitorijum;
            this.faktureRepozitorijum = faktureRepozitorijum;
            this.loggerServis = loggerServis;
=======
            this.paleteRepozitorijum = paleteRepozitorijum ?? throw new ArgumentNullException(nameof(paleteRepozitorijum));   //ne valja
            this.faktureRepozitorijum = faktureRepozitorijum ?? throw new ArgumentNullException(nameof(faktureRepozitorijum));
            this.loggerServis = loggerServis ?? throw new ArgumentNullException(nameof(loggerServis));
>>>>>>> b8d76ad952313d624b62bfabd14004a7c3b0061d
        }

        public Guid IsporuciVinoKupcu(
            Guid paletaId,
            string kupac,
            decimal cenaPoKomadu,
            TipProdaje tipProdaje,
            NacinPlacanja nacinPlacanja)
        {
            if (paleteRepozitorijum == null || faktureRepozitorijum == null || loggerServis == null)
                return Guid.Empty;

            if (paletaId == Guid.Empty)
                return Guid.Empty;

            if (kupac == null || kupac.Trim().Length == 0)
                return Guid.Empty;

            if (cenaPoKomadu <= 0)
                return Guid.Empty;

            Paleta paleta = paleteRepozitorijum.PronadjiPaletuPoId(paletaId);
            if (paleta == null || paleta.Id == Guid.Empty)
                return Guid.Empty;

            if (paleta.Status != StatusPalete.Otpremljena && paleta.Status != StatusPalete.Raspakovana)
            {
                loggerServis.Evidentiraj(
                    TipEvidencije.ERROR,
                    "Pokusaj prodaje palete koja nije spremna. Paleta=" + paleta.Sifra + ", Status=" + paleta.Status
                );
                return Guid.Empty;
            }

            if (paleta.VinaIds == null || paleta.VinaIds.Count == 0)
                return Guid.Empty;

            var stavke = paleta.VinaIds
                .GroupBy(id => id)
                .Select(g => new StavkaFakture(g.Key, g.Count(), cenaPoKomadu))
                .ToList();

            if (stavke == null || stavke.Count == 0)
                return Guid.Empty;

            Faktura faktura = new Faktura
            {
                TipProdaje = tipProdaje,
                NacinPlacanja = nacinPlacanja,
                Stavke = stavke
            };

            var sacuvana = faktureRepozitorijum.DodajFakturu(faktura);
            if (sacuvana == null)
                return Guid.Empty;

            paleta.Status = StatusPalete.Isporucena;
            bool ok = paleteRepozitorijum.AzurirajPaletu(paleta);
            if (!ok)
                return Guid.Empty;

            loggerServis.Evidentiraj(
                TipEvidencije.INFO,
                "Isporucena paleta " + paleta.Sifra + " kupcu " + kupac +
                ". Faktura=" + faktura.Id +
                ", TipProdaje=" + tipProdaje +
                ", Placanje=" + nacinPlacanja
            );

            return faktura.Id;
        }
    }
}
