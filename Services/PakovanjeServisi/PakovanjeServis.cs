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

        public PakovanjeServis(
            IVinoRepozitorijum vinoRepo,
            IPaleteRepozitorijum paleteRepo,
            ISkladistenjeServis skladistenjeServis,
            ILoggerServis logger)
        {
            this.vinoRepo = vinoRepo;
            this.paleteRepo = paleteRepo;
            this.skladistenjeServis = skladistenjeServis;
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

                // 1) Nadji JEDNO vino koje odgovara (u bazi imas 1 zapis po tipu vina)
                Vino? pronadjeno = null;

                foreach (var vino in vinoRepo.PronadjiVinaPoKategoriji(kategorija))
                {
                    string nazivIzBaze = (vino.Naziv ?? string.Empty).Trim();

                    bool istiNaziv = string.Equals(nazivIzBaze, trazeniNaziv, StringComparison.OrdinalIgnoreCase);
                    bool istaZapremina = Math.Abs(vino.ZapreminaLitara - zapreminaFlase) < 0.0001;

                    if (istiNaziv && istaZapremina)
                    {
                        pronadjeno = vino;
                        break;
                    }
                }

                if (pronadjeno == null || pronadjeno.Id == Guid.Empty)
                {
                    logger.Evidentiraj(TipEvidencije.WARNING, "Pakovanje: vino ne postoji (naziv/kategorija/zapremina ne poklapa).");
                    return (false, new Paleta());
                }

                // 2) Formiraj paletu
                Paleta paleta = new Paleta
                {
                    Id = Guid.NewGuid(),
                    Sifra = $"PL-{DateTime.Now:yyyyMMddHHmmss}",
                    AdresaOdredista = adresa,
                    VinskiPodrumId = vinskiPodrumId,
                    Status = StatusPalete.Upakovana,
                    VinaIds = new List<Guid>()
                };

                // 3) Dodaj isti ID onoliko puta koliko je broj flasa
                for (int i = 0; i < brojFlasa; i++)
                    paleta.VinaIds.Add(pronadjeno.Id);

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
