using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.VinogradServisi
{
    public class PaleteServis : IPaleteServis
    {
        private const int MAX_PALETE_PO_ISPORUCI = 5;
        private const int PRIPREMA_MS = 300; // 0.3s

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
            if (brojPaleta <= 0)
                throw new ArgumentException("Broj paleta mora biti veci od 0.");

            if (brojPaleta > MAX_PALETE_PO_ISPORUCI)
                throw new ArgumentException($"U jednoj isporuci je moguce poslati najvise {MAX_PALETE_PO_ISPORUCI} paleta.");

            var podrum = vinskiPodrumRepozitorijum.PronadjiVinskiPodrumPoId(vinskiPodrumId);
            if (podrum == null)
            {
                loggerServis.Evidentiraj(TipEvidencije.ERROR,
                    $"Nepostojeci vinski podrum. ID={vinskiPodrumId}");
                throw new ArgumentException("Nepostojeci vinski podrum.");
            }

            loggerServis.Evidentiraj(
                TipEvidencije.INFO,
                $"Pocetak isporuke: podrum={podrum.Naziv} (ID={vinskiPodrumId}), brojPaleta={brojPaleta}"
            );

            var poslate = new List<Paleta>();

            for (int i = 0; i < brojPaleta; i++)
            {
                // prva dostupna upakovana paleta
                var paleta = NadjiPrvuUpakovanu();

                // ako nema, zapocinjem novu paletu
                if (paleta == null)
                {
                    loggerServis.Evidentiraj(
                        TipEvidencije.WARNING,
                        "Nema dostupne upakovane palete -> kreiram novu paletu (zapocinjem pakovanje)."
                    );

                    paleta = KreirajNovuUpakovanuPaletu();
                    paleteRepozitorijum.DodajPaletu(paleta);  //da li je uspjesno dodata?
                }

                
                Thread.Sleep(PRIPREMA_MS);

                
                paleta.VinskiPodrumId = vinskiPodrumId;
                paleta.Status = StatusPalete.Otpremljena; 

                bool ok = paleteRepozitorijum.AzurirajPaletu(paleta);
                if (!ok)
                {
                    loggerServis.Evidentiraj(
                        TipEvidencije.ERROR,
                        $"Neuspelo azuriranje palete {paleta.Sifra} (ID={paleta.Id})."
                    );
                    throw new InvalidOperationException("Neuspelo azuriranje palete.");
                }

                poslate.Add(paleta);

                loggerServis.Evidentiraj(
                    TipEvidencije.INFO,
                    $"Otpremljena paleta {paleta.Sifra} u podrum '{podrum.Naziv}' (ID={vinskiPodrumId})."
                );
            }

            loggerServis.Evidentiraj(
                TipEvidencije.INFO,
                $"Kraj isporuke: ukupno poslate={poslate.Count}"
            );

            return poslate;
        }

        private Paleta? NadjiPrvuUpakovanu()
        {
            var upakovane = paleteRepozitorijum.PronadjiPaletePoStatusu(StatusPalete.Upakovana);
            return upakovane.FirstOrDefault();
        }

        private Paleta KreirajNovuUpakovanuPaletu()
        {
            return new Paleta
            {
                Id = Guid.NewGuid(),
                Sifra = $"PL-{DateTime.Now:yyyy}-{Guid.NewGuid().ToString()[..6].ToUpper()}",
                Status = StatusPalete.Upakovana,
                VinskiPodrumId = Guid.Empty
            };
        }

        public Paleta KreirajNovuPaletu(string adresaOdredista)
        {
            if (string.IsNullOrWhiteSpace(adresaOdredista))
                throw new ArgumentException("Adresa odredišta je obavezna.");

            Paleta paleta = KreirajNovuUpakovanuPaletu();

            paleta.AdresaOdredista = adresaOdredista.Trim();

            Paleta sacuvana = paleteRepozitorijum.DodajPaletu(paleta);

            if (sacuvana == null)
                throw new InvalidOperationException("Paleta nije sačuvana.");

            return sacuvana;
        }

        public Paleta PregledPalete(Guid paletaId)
        {
            return paleteRepozitorijum.PronadjiPaletuPoId(paletaId);
        }

        public bool DodajProizvedenoVinoNaPaletu(Guid paletaId, Guid evidencijaProizvodnjeVinaId)
        {
            var paleta = paleteRepozitorijum.PronadjiPaletuPoId(paletaId);
            if (paleta == null || paleta.Id == Guid.Empty)
                return false;

            var evidencije = evidencijaVinaRepo.SveEvidencije();
            var evidencija = evidencije.FirstOrDefault(e => e.Id == evidencijaProizvodnjeVinaId);
            if (evidencija == null)
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
