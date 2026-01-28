using System;
using System.Collections.Generic;
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

                List<Vino> dostupnaVina = new List<Vino>();

                foreach (var vino in vinoRepo.PronadjiVinaPoKategoriji(kategorija))
                {
                    if (vino.Naziv == nazivVina &&
                        vino.ZapreminaLitara == zapreminaFlase)
                    {
                        dostupnaVina.Add(vino);
                        if (dostupnaVina.Count == brojFlasa)
                            break;
                    }
                }

                if (dostupnaVina.Count < brojFlasa)
                {
                    logger.Evidentiraj(TipEvidencije.WARNING, "Pakovanje: nema dovoljno vina.");
                    return (false, new Paleta());
                }

                Paleta paleta = new Paleta
                {
                    Id = Guid.NewGuid(),
                    Sifra = $"PL-{DateTime.Now:yyyyMMddHHmmss}",
                    AdresaOdredista = adresaOdredista,
                    VinskiPodrumId = vinskiPodrumId,
                    Status = StatusPalete.Upakovana,
                    VinaIds = new List<Guid>()
                };

                foreach (var vino in dostupnaVina)
                    paleta.VinaIds.Add(vino.Id);

                paleteRepo.DodajPaletu(paleta);

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

                Paleta paleta = new Paleta();


                foreach (var p in paleteRepo.PronadjiPaletePoStatusu(StatusPalete.Upakovana))
                {
                    if (p.AdresaOdredista == adresaOdredista &&
                        p.VinskiPodrumId == vinskiPodrumId)
                    {
                        paleta = p;
                        break;
                    }
                }

                if (paleta == null)
                {
                    logger.Evidentiraj(
                        TipEvidencije.INFO,
                        "Pakovanje: nema palete, pokusavam novo pakovanje."
                    );

                    var rezultat = UpakujVinaUPaletu(
                        nazivVina,
                        kategorija,
                        brojFlasa,
                        zapreminaFlase,
                        adresaOdredista,
                        vinskiPodrumId
                    );

                    if (!rezultat.Item1)
                        return (false, new Paleta());

                    paleta = rezultat.Item2;
                }

                paleta.Status = StatusPalete.Otpremljena;
                paleteRepo.AzurirajPaletu(paleta);

                bool prihvaceno = skladistenjeServis.PrihvatiOtpremljenuPaletu(paleta);
                if (!prihvaceno)
                {
                    paleta.Status = StatusPalete.Upakovana;
                    paleteRepo.AzurirajPaletu(paleta);

                    logger.Evidentiraj(
                        TipEvidencije.ERROR,
                        "Pakovanje: skladiste nije prihvatilo paletu."
                    );

                    return (false, new Paleta());
                }

                logger.Evidentiraj(
                    TipEvidencije.INFO,
                    "Pakovanje: paleta uspjesno poslata u skladiste."
                );

                return (true, paleta);
            }
            catch (Exception ex)
            {
                logger.Evidentiraj(
                    TipEvidencije.ERROR,
                    "Pakovanje: greska pri slanju - " + ex.Message
                );

                return (false, new Paleta());
            }




        }
    }
}
