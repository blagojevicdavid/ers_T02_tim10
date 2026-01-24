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

        public PaleteServis(
            IPaleteRepozitorijum paleteRepozitorijum,
            IVinskiPodrumRepozitorijum vinskiPodrumRepozitorijum,
            ILoggerServis loggerServis)
        {
            this.paleteRepozitorijum = paleteRepozitorijum;
            this.vinskiPodrumRepozitorijum = vinskiPodrumRepozitorijum;
            this.loggerServis = loggerServis;
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
                    $"[SCRUM-81] Nepostojeci vinski podrum. ID={vinskiPodrumId}");
                throw new ArgumentException("Nepostojeci vinski podrum.");
            }

            loggerServis.Evidentiraj(
                TipEvidencije.INFO,
                $"[SCRUM-81] Pocetak isporuke: podrum={podrum.Naziv} (ID={vinskiPodrumId}), brojPaleta={brojPaleta}"
            );

            var poslate = new List<Paleta>();

            for (int i = 0; i < brojPaleta; i++)
            {
                // 1) prva dostupna upakovana paleta
                var paleta = NadjiPrvuUpakovanu();

                // 2) ako nema, zapocni novu paletu
                if (paleta == null)
                {
                    loggerServis.Evidentiraj(
                        TipEvidencije.WARNING,
                        "[SCRUM-81] Nema dostupne upakovane palete -> kreiram novu paletu (zapocinjem pakovanje)."
                    );

                    paleta = KreirajNovuUpakovanuPaletu();
                    paleteRepozitorijum.DodajPaletu(paleta);
                }

                // 3) priprema 0.3s po paleti
                Thread.Sleep(PRIPREMA_MS);

                // 4) oznaci kao otpremljenu i dodijeli podrum
                paleta.VinskiPodrumId = vinskiPodrumId;
                paleta.Status = StatusPalete.Otpremljena; // ✅ tacno kao u tvom enum-u

                bool ok = paleteRepozitorijum.AzurirajPaletu(paleta);
                if (!ok)
                {
                    loggerServis.Evidentiraj(
                        TipEvidencije.ERROR,
                        $"[SCRUM-81] Neuspelo azuriranje palete {paleta.Sifra} (ID={paleta.Id})."
                    );
                    throw new InvalidOperationException("Neuspelo azuriranje palete.");
                }

                poslate.Add(paleta);

                loggerServis.Evidentiraj(
                    TipEvidencije.INFO,
                    $"[SCRUM-81] Otpremljena paleta {paleta.Sifra} u podrum '{podrum.Naziv}' (ID={vinskiPodrumId})."
                );
            }

            loggerServis.Evidentiraj(
                TipEvidencije.INFO,
                $"[SCRUM-81] Kraj isporuke: ukupno poslate={poslate.Count}"
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

    }
}
