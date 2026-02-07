using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.PakovanjeServisi
{
    public class PakovanjeServis : IPakovanjeServis
    {
        private readonly IVinoRepozitorijum vinoRepo;
        private readonly IPaleteRepozitorijum paleteRepo;
        private readonly ISkladistenjeServis skladistenjeServis;
        private readonly ILoggerServis logger;
        private readonly IProizvodnjaVinaServis proizvodnjaServis;

        public PakovanjeServis(
            IVinoRepozitorijum vinoRepo,
            IPaleteRepozitorijum paleteRepo,
            ISkladistenjeServis skladistenjeServis,
            IProizvodnjaVinaServis proizvodnjaServis,
            ILoggerServis logger)
        {
            this.vinoRepo = vinoRepo;
            this.paleteRepo = paleteRepo;
            this.skladistenjeServis = skladistenjeServis;
            this.proizvodnjaServis = proizvodnjaServis;
            this.logger = logger;
        }

        public (bool, Paleta) UpakujVinaUPaletu(
            string nazivVina,
            KategorijaVina kategorija,
            int brojFlasa,
            double zapreminaFlase,
            string adresaOdredista,
            Guid vinskiPodrumId)
        {
            if (paleteRepo == null || skladistenjeServis == null || proizvodnjaServis == null || logger == null)
                return (false, new Paleta());

            logger.Evidentiraj(TipEvidencije.INFO, "Pakovanje: zapoceto pakovanje vina.");

            if (brojFlasa <= 0)
            {
                logger.Evidentiraj(TipEvidencije.WARNING, "Pakovanje: broj flasa mora biti veci od 0.");
                return (false, new Paleta());
            }

            if (zapreminaFlase <= 0)
            {
                logger.Evidentiraj(TipEvidencije.WARNING, "Pakovanje: zapremina flase mora biti veca od 0.");
                return (false, new Paleta());
            }

            if (vinskiPodrumId == Guid.Empty)
                return (false, new Paleta());

            string trazeniNaziv = nazivVina == null ? string.Empty : nazivVina.Trim();
            string adresa = adresaOdredista == null ? string.Empty : adresaOdredista.Trim();

            if (trazeniNaziv.Length == 0 || adresa.Length == 0)
                return (false, new Paleta());

            var proizvedena = proizvodnjaServis.ProizvediVina(trazeniNaziv, kategorija, brojFlasa, zapreminaFlase);

            if (proizvedena == null || proizvedena.Count < brojFlasa)
                return (false, new Paleta());

            for (int i = 0; i < brojFlasa; i++)
            {
                if (proizvedena[i] == null || proizvedena[i].Id == Guid.Empty)
                    return (false, new Paleta());
            }

            Paleta paleta = new Paleta
            {
                Id = Guid.NewGuid(),
                Sifra = "PL-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                AdresaOdredista = adresa,
                VinskiPodrumId = vinskiPodrumId,
                Status = StatusPalete.Upakovana,
                VinaIds = new List<Guid>(brojFlasa)
            };

            for (int i = 0; i < brojFlasa; i++)
                paleta.VinaIds.Add(proizvedena[i].Id);

            var sacuvana = paleteRepo.DodajPaletu(paleta);
            if (sacuvana == null)
            {
                logger.Evidentiraj(TipEvidencije.ERROR, "Pakovanje: neuspelo cuvanje palete u repozitorijum.");
                return (false, new Paleta());
            }

            logger.Evidentiraj(
                TipEvidencije.INFO,
                "Pakovanje: paleta formirana (" + paleta.Sifra + "), broj flasa = " + paleta.VinaIds.Count
            );

            return (true, paleta);
        }

        public (bool, Paleta) PosaljiPrvuDostupnuUpakovanuPaletu(
            string nazivVina,
            KategorijaVina kategorija,
            int brojFlasa,
            double zapreminaFlase,
            string adresaOdredista,
            Guid vinskiPodrumId)
        {
            if (paleteRepo == null || skladistenjeServis == null || proizvodnjaServis == null || logger == null)
                return (false, new Paleta());

            logger.Evidentiraj(TipEvidencije.INFO, "Pakovanje: trazenje upakovane palete.");

            if (vinskiPodrumId == Guid.Empty)
                return (false, new Paleta());

            string adresa = adresaOdredista == null ? string.Empty : adresaOdredista.Trim();
            if (adresa.Length == 0)
                return (false, new Paleta());

            Paleta paleta = null;

            var lista = paleteRepo.PronadjiPaletePoStatusu(StatusPalete.Upakovana);
            if (lista != null)
            {
                foreach (var p in lista)
                {
                    if (p == null) continue;

                    string pAdresa = p.AdresaOdredista == null ? string.Empty : p.AdresaOdredista.Trim();

                    if (pAdresa.Equals(adresa, StringComparison.OrdinalIgnoreCase) &&
                        p.VinskiPodrumId == vinskiPodrumId)
                    {
                        paleta = p;
                        break;
                    }
                }
            }

            if (paleta == null)
            {
                logger.Evidentiraj(TipEvidencije.INFO, "Pakovanje: nema palete, pokusavam novo pakovanje.");

                var rezultat = UpakujVinaUPaletu(
                    nazivVina,
                    kategorija,
                    brojFlasa,
                    zapreminaFlase,
                    adresa,
                    vinskiPodrumId
                );

                if (!rezultat.Item1)
                    return (false, new Paleta());

                paleta = rezultat.Item2;

                if (paleta == null)
                    return (false, new Paleta());
            }

            if (paleta.Id == Guid.Empty)
                paleta.Id = Guid.NewGuid();

            paleta.Status = StatusPalete.Otpremljena;

            bool okAzur = paleteRepo.AzurirajPaletu(paleta);
            if (!okAzur)
            {
                logger.Evidentiraj(TipEvidencije.ERROR, "Pakovanje: neuspelo azuriranje palete na Otpremljena.");
                return (false, new Paleta());
            }

            bool prihvaceno = skladistenjeServis.PrihvatiOtpremljenuPaletu(paleta);
            if (!prihvaceno)
            {
                paleta.Status = StatusPalete.Upakovana;
                paleteRepo.AzurirajPaletu(paleta);

                logger.Evidentiraj(TipEvidencije.ERROR, "Pakovanje: skladiste nije prihvatilo paletu.");
                return (false, new Paleta());
            }

            logger.Evidentiraj(TipEvidencije.INFO, "Pakovanje: paleta uspjesno poslata u skladiste.");
            return (true, paleta);
        }
    }
}
