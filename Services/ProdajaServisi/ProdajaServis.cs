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
            this.paleteRepozitorijum = paleteRepozitorijum;
            this.faktureRepozitorijum = faktureRepozitorijum;
            this.loggerServis = loggerServis;
        }

        public Guid IsporuciVinoKupcu(Guid paletaId, string kupac, decimal cenaPoKomadu)
        {
            // 1) validacija ulaza
            if (paletaId == Guid.Empty)
                throw new ArgumentException("PaletaId je obavezan.");

            if (string.IsNullOrWhiteSpace(kupac))
                throw new ArgumentException("Kupac je obavezan.");

            if (cenaPoKomadu <= 0)
                throw new ArgumentException("Cena po komadu mora biti > 0.");

            // 2) ucitaj paletu
            Paleta paleta = paleteRepozitorijum.PronadjiPaletuPoId(paletaId);
            if (paleta == null)
                throw new InvalidOperationException("Paleta ne postoji.");

            // 3) mora biti otpremljena da bi se isporucila kupcu
            if (paleta.Status != StatusPalete.Otpremljena)
            {
                loggerServis.Evidentiraj(
                    TipEvidencije.ERROR,
                    $"Pokušaj isporuke palete koja nije spremna. Paleta={paleta.Sifra}, Status={paleta.Status}"
                );

                throw new InvalidOperationException("Paleta nije spremna za isporuku (mora biti Otpremljena).");
            }

            if (paleta.VinaIds == null || paleta.VinaIds.Count == 0)
                throw new InvalidOperationException("Paleta nema vina.");

            // 4) napravi stavke fakture iz VinaIds (grupisanje po vinu)
            var stavke = paleta.VinaIds
                .GroupBy(id => id)
                .Select(g => new StavkaFakture(g.Key, g.Count(), cenaPoKomadu))
                .ToList();

            // 5) kreiraj i sacuvaj fakturu
            Faktura faktura = new Faktura
            {
                Stavke = stavke
            };

            faktureRepozitorijum.DodajFakturu(faktura);

            // 6) azuriraj status palete -> ISPORUCENA (SCRUM-87)
            paleta.Status = StatusPalete.Isporucena;
            paleteRepozitorijum.AzurirajPaletu(paleta);

            // 7) log
            loggerServis.Evidentiraj(
                TipEvidencije.INFO,
                $" Isporucena paleta {paleta.Sifra} kupcu {kupac}. Faktura={faktura.Id}"
            );

            return faktura.Id;
        }
    }
}
