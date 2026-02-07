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
        private const int MaxPaletaPoIsporuci = 5;
        private const double SekundiPoPaleti = 0.3;

        private readonly IPaleteRepozitorijum paleteRepo;
        private readonly ILoggerServis logger;

        private NacinSkladistenja izabraniNacin;
        private Guid izabraniVinskiPodrumId;
        private Guid izabraniLokalniPodrumId;

        private bool imaNacin;
        private bool imaVinski;
        private bool imaLokalni;

        public VinskiPodrumSkladistenjeServis(IPaleteRepozitorijum paleteRepo, ILoggerServis logger)
        {
            this.paleteRepo = paleteRepo;
            this.logger = logger;

            imaNacin = false;
            imaVinski = false;
            imaLokalni = false;

            izabraniVinskiPodrumId = Guid.Empty;
            izabraniLokalniPodrumId = Guid.Empty;
        }

        public void PostaviNacinSkladistenja(NacinSkladistenja nacin)
        {
            izabraniNacin = nacin;
            imaNacin = true;
        }

        public NacinSkladistenja PreuzmiNacinSkladistenja()
        {
            if (!imaNacin) return default(NacinSkladistenja);
            return izabraniNacin;
        }

        public void PostaviVinskiPodrum(Guid vinskiPodrumId)
        {
            if (vinskiPodrumId == Guid.Empty) return;

            izabraniVinskiPodrumId = vinskiPodrumId;
            imaVinski = true;

            imaLokalni = false;
            izabraniLokalniPodrumId = Guid.Empty;
        }

        public Guid PreuzmiVinskiPodrum()
        {
            if (!imaVinski) return Guid.Empty;
            return izabraniVinskiPodrumId;
        }

        public void PostaviLokalniPodrum(Guid lokalniPodrumId)
        {
            if (lokalniPodrumId == Guid.Empty) return;

            izabraniLokalniPodrumId = lokalniPodrumId;
            imaLokalni = true;

            imaVinski = false;
            izabraniVinskiPodrumId = Guid.Empty;
        }

        public Guid PreuzmiLokalniPodrum()
        {
            if (!imaLokalni) return Guid.Empty;
            return izabraniLokalniPodrumId;
        }

        public bool PrihvatiOtpremljenuPaletu(Paleta paleta)
        {
            if (paleteRepo == null || logger == null) return false;

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
                logger.Evidentiraj(TipEvidencije.WARNING, "VinskiPodrum: paleta nije otpremljena (Status=" + izRepo.Status + ").");
                return false;
            }

            logger.Evidentiraj(TipEvidencije.INFO, "VinskiPodrum: paleta prihvacena (Sifra=" + izRepo.Sifra + ").");
            return true;
        }

        public IEnumerable<Paleta> IsporuciPaleteZaProdaju(int brojPaleta)
        {
            if (paleteRepo == null || logger == null) return new List<Paleta>();

            logger.Evidentiraj(TipEvidencije.INFO, "VinskiPodrum: zahtev za isporuku paleta (Trazeno=" + brojPaleta + ").");

            if (brojPaleta <= 0)
            {
                logger.Evidentiraj(TipEvidencije.WARNING, "VinskiPodrum: brojPaleta mora biti > 0.");
                return new List<Paleta>();
            }

            int trazeno = brojPaleta;
            if (trazeno > MaxPaletaPoIsporuci) trazeno = MaxPaletaPoIsporuci;

            if (trazeno != brojPaleta)
                logger.Evidentiraj(TipEvidencije.WARNING, "VinskiPodrum: trazeno vise od limita, smanjujem na " + trazeno + ".");

            List<Paleta> rezultat = new List<Paleta>();

            var lista = paleteRepo.PronadjiPaletePoStatusu(StatusPalete.Otpremljena);
            if (lista != null)
            {
                foreach (Paleta p in lista)
                {
                    if (rezultat.Count >= trazeno) break;
                    if (p == null || p.Id == Guid.Empty) continue;

                    p.Status = StatusPalete.Raspakovana;

                    bool ok = paleteRepo.AzurirajPaletu(p);
                    if (!ok)
                    {
                        logger.Evidentiraj(TipEvidencije.ERROR, "VinskiPodrum: neuspesno azuriranje palete (Sifra=" + p.Sifra + ").");
                        continue;
                    }

                    rezultat.Add(p);
                    logger.Evidentiraj(TipEvidencije.INFO, "VinskiPodrum: paleta raspakovana (Sifra=" + p.Sifra + ").");
                }
            }

            int ms = (int)(rezultat.Count * SekundiPoPaleti * 1000);
            if (ms > 0)
            {
                logger.Evidentiraj(TipEvidencije.INFO, "VinskiPodrum: priprema paleta (ms=" + ms + ").");
                Thread.Sleep(ms);
            }

            logger.Evidentiraj(TipEvidencije.INFO, "VinskiPodrum: isporuka zavrsena (Isporuceno=" + rezultat.Count + ").");
            return rezultat;
        }
    }
}
