using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Threading;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.SkladistenjeServisi
{
    public class VinskiPodrumSkladistenjeServis : ISkladistenjeServis
    {
        private const int MaxPaletaPoIsporuci = 5;     // spec: do 5
        private const double SekundiPoPaleti = 0.3;    // spec: 0.3s po paleti

        private readonly IPaleteRepozitorijum paleteRepo;
        private readonly ILoggerServis logger;

        public VinskiPodrumSkladistenjeServis(IPaleteRepozitorijum paleteRepo, ILoggerServis logger)
        {
            this.paleteRepo = paleteRepo;
            this.logger = logger;
        }

        public bool PrihvatiOtpremljenuPaletu(Paleta paleta)
        {
            try
            {
                logger.Evidentiraj(TipEvidencije.INFO, "VinskiPodrum: pokusaj prihvatanja otpremljene palete.");

                if (paleta == null || paleta.Id == Guid.Empty)
                {
                    logger.Evidentiraj(TipEvidencije.WARNING, "VinskiPodrum: paleta je null ili nema validan Id.");
                    return false;
                }

                Paleta izRepo = paleteRepo.PronadjiPaletuPoId(paleta.Id);
                if (izRepo == null || izRepo.Id == Guid.Empty)
                {
                    logger.Evidentiraj(TipEvidencije.WARNING, "VinskiPodrum: paleta ne postoji u repozitorijumu.");
                    return false;
                }

                if (izRepo.Status != StatusPalete.Otpremljena)
                {
                    logger.Evidentiraj(TipEvidencije.WARNING, $"VinskiPodrum: paleta nije otpremljena (Status={izRepo.Status}).");
                    return false;
                }

                logger.Evidentiraj(TipEvidencije.INFO, $"VinskiPodrum: paleta prihvacena (Sifra={izRepo.Sifra}).");
                return true;
            }
            catch (Exception ex)
            {
                logger.Evidentiraj(TipEvidencije.ERROR, "VinskiPodrum: greska pri prihvatanju palete - " + ex.Message);
                return false;
            }
        }

        public IEnumerable<Paleta> IsporuciPaleteZaProdaju(int brojPaleta)
        {
            try
            {
                logger.Evidentiraj(TipEvidencije.INFO, $"VinskiPodrum: zahtev za isporuku paleta (Trazeno={brojPaleta}).");

                if (brojPaleta <= 0)
                {
                    logger.Evidentiraj(TipEvidencije.WARNING, "VinskiPodrum: brojPaleta mora biti > 0.");
                    return new List<Paleta>();
                }

                int trazeno = Math.Min(brojPaleta, MaxPaletaPoIsporuci);
                if (trazeno != brojPaleta)
                    logger.Evidentiraj(TipEvidencije.WARNING, $"VinskiPodrum: trazeno vise od limita, smanjujem na {trazeno}.");

                List<Paleta> rezultat = new List<Paleta>();

                foreach (Paleta p in paleteRepo.PronadjiPaletePoStatusu(StatusPalete.Otpremljena))
                {
                    if (rezultat.Count >= trazeno)
                        break;

                    if (p == null || p.Id == Guid.Empty)
                        continue;

                    // Raspakuj za prodaju
                    p.Status = StatusPalete.Raspakovana;

                    bool ok = paleteRepo.AzurirajPaletu(p);
                    if (!ok)
                    {
                        logger.Evidentiraj(TipEvidencije.ERROR, $"VinskiPodrum: neuspesno azuriranje palete (Sifra={p.Sifra}).");
                        continue;
                    }

                    rezultat.Add(p);
                    logger.Evidentiraj(TipEvidencije.INFO, $"VinskiPodrum: paleta raspakovana (Sifra={p.Sifra}).");
                }

                int ms = (int)(rezultat.Count * SekundiPoPaleti * 1000);
                if (ms > 0)
                {
                    logger.Evidentiraj(TipEvidencije.INFO, $"VinskiPodrum: priprema paleta (ms={ms}).");
                    Thread.Sleep(ms);
                }

                logger.Evidentiraj(TipEvidencije.INFO, $"VinskiPodrum: isporuka zavrsena (Isporuceno={rezultat.Count}).");
                return rezultat;
            }
            catch (Exception ex)
            {
                logger.Evidentiraj(TipEvidencije.ERROR, "VinskiPodrum: greska pri isporuci paleta - " + ex.Message);
                return new List<Paleta>();
            }
        }
    }
}
