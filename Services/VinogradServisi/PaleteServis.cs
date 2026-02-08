using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Konstante;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.VinogradServisi
{
    public class PaleteServis : IPaleteServis
    {
        private readonly IPaleteRepozitorijum paleteRepozitorijum;
        private readonly IVinskiPodrumRepozitorijum vinskiPodrumRepozitorijum;
        private readonly ILoggerServis loggerServis;
        private readonly IEvidencijaProizvodnjeVinaRepozitorijum evidencijaVinaRepo;

        public PaleteServis(
            IPaleteRepozitorijum paleteRepozitorijum,
            IVinskiPodrumRepozitorijum vinskiPodrumRepozitorijum,
            ILoggerServis loggerServis,
            IEvidencijaProizvodnjeVinaRepozitorijum evidencijaVinaRepo)
        {
            this.paleteRepozitorijum = paleteRepozitorijum;
            this.vinskiPodrumRepozitorijum = vinskiPodrumRepozitorijum;
            this.loggerServis = loggerServis;
            this.evidencijaVinaRepo = evidencijaVinaRepo;
        }

        public IList<Paleta> PosaljiPaleteUVinskiPodrum(Guid vinskiPodrumId, int brojPaleta)
        {
            if (paleteRepozitorijum == null || vinskiPodrumRepozitorijum == null || loggerServis == null)
                return new List<Paleta>();

            if (vinskiPodrumId == Guid.Empty)
                return new List<Paleta>();

            if (brojPaleta <= 0)
            {
                loggerServis.Evidentiraj(TipEvidencije.ERROR, "[PALETE] Nevalidan broj paleta: " + brojPaleta);
                return new List<Paleta>();
            }

            if (brojPaleta > PaleteKonstante.MAX_PALETE_PO_ISPORUCI)
            {
                loggerServis.Evidentiraj(
                    TipEvidencije.ERROR,
                    "[PALETE] Prekoracen limit po isporuci: trazeno=" + brojPaleta + ", max=" + PaleteKonstante.MAX_PALETE_PO_ISPORUCI
                );
                return new List<Paleta>();
            }

            var podrum = vinskiPodrumRepozitorijum.PronadjiVinskiPodrumPoId(vinskiPodrumId);
            if (podrum == null)
            {
                loggerServis.Evidentiraj(TipEvidencije.ERROR, "Nepostojeci vinski podrum. ID=" + vinskiPodrumId);
                return new List<Paleta>();
            }

            loggerServis.Evidentiraj(
                TipEvidencije.INFO,
                "Pocetak isporuke: podrum=" + podrum.Naziv + " (ID=" + vinskiPodrumId + "), brojPaleta=" + brojPaleta
            );

            var poslate = new List<Paleta>();

            for (int i = 0; i < brojPaleta; i++)
            {
                var paleta = NadjiPrvuUpakovanu();

                if (paleta == null || paleta.Id == Guid.Empty)
                {
                    loggerServis.Evidentiraj(
                        TipEvidencije.WARNING,
                        "Nema dostupne upakovane palete -> kreiram novu paletu (zapocinjem pakovanje)."
                    );

                    paleta = KreirajNovuUpakovanuPaletu();
                    if (paleta == null || paleta.Id == Guid.Empty)
                        return new List<Paleta>();

                    var sacuvana = paleteRepozitorijum.DodajPaletu(paleta);
                    if (sacuvana == null || sacuvana.Id == Guid.Empty)
                    {
                        loggerServis.Evidentiraj(TipEvidencije.ERROR, "[PALETE] Neuspelo kreiranje nove palete tokom slanja u podrum.");
                        return new List<Paleta>();
                    }

                    paleta = sacuvana;
                }

                Thread.Sleep(PaleteKonstante.PRIPREMA_MS);

                paleta.VinskiPodrumId = vinskiPodrumId;
                paleta.Status = StatusPalete.Otpremljena;

                bool ok = paleteRepozitorijum.AzurirajPaletu(paleta);
                if (!ok)
                {
                    loggerServis.Evidentiraj(TipEvidencije.ERROR, "Neuspelo azuriranje palete " + paleta.Sifra + " (ID=" + paleta.Id + ").");
                    return new List<Paleta>();
                }

                poslate.Add(paleta);

                loggerServis.Evidentiraj(
                    TipEvidencije.INFO,
                    "Otpremljena paleta " + paleta.Sifra + " u podrum '" + podrum.Naziv + "' (ID=" + vinskiPodrumId + ")."
                );
            }

            loggerServis.Evidentiraj(TipEvidencije.INFO, "Kraj isporuke: ukupno poslate=" + poslate.Count);

            return poslate;
        }

        private Paleta NadjiPrvuUpakovanu()
        {
            if (paleteRepozitorijum == null)
                return new Paleta();

            var upakovane = paleteRepozitorijum.PronadjiPaletePoStatusu(StatusPalete.Upakovana);
            if (upakovane == null)
                return new Paleta();

            var prva = upakovane.FirstOrDefault();
            if (prva == null)
                return new Paleta();

            return prva;
        }


        private Paleta KreirajNovuUpakovanuPaletu()
        {
            return new Paleta
            {
                Id = Guid.NewGuid(),
                Sifra = "PL-" + DateTime.Now.ToString("yyyy") + "-" + Guid.NewGuid().ToString().Substring(0, 6).ToUpper(),
                Status = StatusPalete.Upakovana,
                VinskiPodrumId = Guid.Empty,
                VinaIds = new List<Guid>()
            };
        }

        public Paleta KreirajNovuPaletu(string adresaOdredista)
        {
            if (paleteRepozitorijum == null || loggerServis == null)
                return new Paleta();

            string adresa;

            if (adresaOdredista == null)
            {
                adresa = string.Empty;
            }
            else
            {
                adresa = adresaOdredista.Trim();
            }

            if (adresa.Length == 0)
            {
                loggerServis.Evidentiraj(TipEvidencije.ERROR, "[PALETE] Kreiranje palete neuspesno – adresa odredista nije unijeta.");
                return new Paleta();
            }


            Paleta paleta = KreirajNovuUpakovanuPaletu();
            if (paleta == null || paleta.Id == Guid.Empty)
                return new Paleta();

            paleta.AdresaOdredista = adresa;

            Paleta sacuvana = paleteRepozitorijum.DodajPaletu(paleta);
            if (sacuvana == null || sacuvana.Id == Guid.Empty)
                return new Paleta();

            return sacuvana;
        }

        public Paleta PregledPalete(Guid paletaId)
        {
            if (paleteRepozitorijum == null) return new Paleta();
            if (paletaId == Guid.Empty) return new Paleta();

            var p = paleteRepozitorijum.PronadjiPaletuPoId(paletaId);
            if (p == null) return new Paleta();

            return p;
        }

        public bool DodajProizvedenoVinoNaPaletu(Guid paletaId, Guid evidencijaProizvodnjeVinaId)
        {
            if (paleteRepozitorijum == null || evidencijaVinaRepo == null || loggerServis == null)
                return false;

            if (paletaId == Guid.Empty || evidencijaProizvodnjeVinaId == Guid.Empty)
                return false;

            var paleta = paleteRepozitorijum.PronadjiPaletuPoId(paletaId);
            if (paleta == null || paleta.Id == Guid.Empty)
            {
                loggerServis.Evidentiraj(TipEvidencije.WARNING, "[PALETE] Dodavanje vina neuspesno – paleta ne postoji (ID=" + paletaId + ").");
                return false;
            }

            var evidencije = evidencijaVinaRepo.SveEvidencije();
            if (evidencije == null)
                return false;

            var evidencija = evidencije.FirstOrDefault(e => e != null && e.Id == evidencijaProizvodnjeVinaId);
            if (evidencija == null || evidencija.Id == Guid.Empty)
                return false;

            if (paleta.VinaIds == null)
                paleta.VinaIds = new List<Guid>();

            if (paleta.VinaIds.Contains(evidencijaProizvodnjeVinaId))
                return false;

            paleta.VinaIds.Add(evidencijaProizvodnjeVinaId);

            return paleteRepozitorijum.AzurirajPaletu(paleta);
        }
    }
}
