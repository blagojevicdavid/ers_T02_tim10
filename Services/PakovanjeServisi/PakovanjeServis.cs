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
            try
            {
                logger.Evidentiraj(TipEvidencije.INFO, "Pakovanje: zapoceto pakovanje vina.");

                if (brojFlasa <= 0)
                {
                    logger.Evidentiraj(TipEvidencije.WARNING, "Pakovanje: broj flasa mora biti veci od 0.");
                    return (false, new Paleta());
                }

                string trazeniNaziv = (nazivVina ?? string.Empty).Trim();
                string adresa = (adresaOdredista ?? string.Empty).Trim();

                var proizvedena = proizvodnjaServis.ProizvediVina(
                    trazeniNaziv,
                    kategorija,
                    brojFlasa,
                    zapreminaFlase
                );

                if (proizvedena == null || proizvedena.Count != brojFlasa)
                    return (false, new Paleta());

                Paleta paleta = new Paleta
                {
                    Id = Guid.NewGuid(),
                    Sifra = $"PL-{DateTime.Now:yyyyMMddHHmmss}",
                    AdresaOdredista = adresa,
                    VinskiPodrumId = vinskiPodrumId,
                    Status = StatusPalete.Upakovana,
                    VinaIds = new List<Guid>()
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
                    $"Pakovanje: paleta formirana ({paleta.Sifra}), broj flasa = {paleta.VinaIds.Count}"
                );

                return (true, paleta);
            }
            catch (Exception ex)
            {
                logger.Evidentiraj(TipEvidencije.ERROR, "Pakovanje: greska - " + ex.Message);
                return (false, new Paleta());
            }
        }


        public (bool, Paleta) PosaljiPrvuDostupnuUpakovanuPaletu(
            string nazivVina,
            KategorijaVina kategorija,
            int brojFlasa,
            double zapreminaFlase,
            string adresaOdredista,
            Guid vinskiPodrumId)
        {
            try
            {
                logger.Evidentiraj(TipEvidencije.INFO, "Pakovanje: trazenje upakovane palete.");

                string adresa = (adresaOdredista ?? string.Empty).Trim();

                // paleta krece kao null
                Paleta? paleta = null;

                foreach (var p in paleteRepo.PronadjiPaletePoStatusu(StatusPalete.Upakovana))
                {
                    if ((p.AdresaOdredista ?? string.Empty).Trim().Equals(adresa, StringComparison.OrdinalIgnoreCase) &&
                        p.VinskiPodrumId == vinskiPodrumId)
                    {
                        paleta = p;
                        break;
                    }
                }

                // ako nema postojece, napravi novu
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
                }

                // otprema
                paleta.Status = StatusPalete.Otpremljena;
                bool okAzur = paleteRepo.AzurirajPaletu(paleta);
                if (!okAzur)
                {
                    logger.Evidentiraj(TipEvidencije.ERROR, "Pakovanje: neuspelo azuriranje palete na Otpremljena.");
                    return (false, new Paleta());
                }

                // skladiste prihvata samo otpremljenu
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
            catch (Exception ex)
            {
                logger.Evidentiraj(TipEvidencije.ERROR, "Pakovanje: greska pri slanju - " + ex.Message);
                return (false, new Paleta());
            }
        }
    }
}
