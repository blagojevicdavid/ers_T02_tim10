using System;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using System.Collections.Generic;
using System.Linq;

namespace Database.BazaPodataka
{
    public static class PocetniPodaci
    {
        private static readonly Guid PodrumA_Id = new Guid("aa111111-bbbb-4444-8888-000000000001");
        private static readonly Guid PodrumB_Id = new Guid("bb222222-cccc-5555-9999-000000000002");
        private static readonly Guid PodrumC_Id = new Guid("cc333333-dddd-6666-aaaa-000000000003");

        private static readonly Guid Loza01_Id = new Guid("11111111-aaaa-4444-8888-000000000010");
        private static readonly Guid Loza02_Id = new Guid("22222222-bbbb-5555-9999-000000000020");
        private static readonly Guid Loza03_Id = new Guid("33333333-cccc-6666-aaaa-000000000030");
        private static readonly Guid Loza04_Id = new Guid("44444444-dddd-7777-bbbb-000000000040");
        private static readonly Guid Loza05_Id = new Guid("55555555-eeee-8888-cccc-000000000050");
        private static readonly Guid Loza06_Id = new Guid("66666666-ffff-9999-dddd-000000000060");
        private static readonly Guid Loza07_Id = new Guid("77777777-1111-2222-3333-000000000070");
        private static readonly Guid Loza08_Id = new Guid("88888888-2222-3333-4444-000000000080");
        private static readonly Guid Loza09_Id = new Guid("99999999-3333-4444-5555-000000000090");
        private static readonly Guid Loza10_Id = new Guid("aaaaaaaa-4444-5555-6666-0000000000a0");
        private static readonly Guid Loza11_Id = new Guid("bbbbbbbb-5555-6666-7777-0000000000b0");
        private static readonly Guid Loza12_Id = new Guid("cccccccc-6666-7777-8888-0000000000c0");
        private static readonly Guid Loza13_Id = new Guid("dddddddd-7777-8888-9999-0000000000d0");
        private static readonly Guid Loza14_Id = new Guid("eeeeeeee-8888-9999-aaaa-0000000000e0");
        private static readonly Guid Loza15_Id = new Guid("ffffffff-9999-aaaa-bbbb-0000000000f0");

        private static readonly Guid Vino01_Id = new Guid("10000000-0000-0000-0000-000000000001");
        private static readonly Guid Vino02_Id = new Guid("10000000-0000-0000-0000-000000000002");
        private static readonly Guid Vino03_Id = new Guid("10000000-0000-0000-0000-000000000003");
        private static readonly Guid Vino04_Id = new Guid("10000000-0000-0000-0000-000000000004");
        private static readonly Guid Vino05_Id = new Guid("10000000-0000-0000-0000-000000000005");
        private static readonly Guid Vino06_Id = new Guid("10000000-0000-0000-0000-000000000006");
        private static readonly Guid Vino07_Id = new Guid("10000000-0000-0000-0000-000000000007");
        private static readonly Guid Vino08_Id = new Guid("10000000-0000-0000-0000-000000000008");
        private static readonly Guid Vino09_Id = new Guid("10000000-0000-0000-0000-000000000009");
        private static readonly Guid Vino10_Id = new Guid("10000000-0000-0000-0000-000000000010");
        private static readonly Guid Vino11_Id = new Guid("10000000-0000-0000-0000-000000000011");
        private static readonly Guid Vino12_Id = new Guid("10000000-0000-0000-0000-000000000012");
        private static readonly Guid Vino13_Id = new Guid("10000000-0000-0000-0000-000000000013");
        private static readonly Guid Vino14_Id = new Guid("10000000-0000-0000-0000-000000000014");
        private static readonly Guid Vino15_Id = new Guid("10000000-0000-0000-0000-000000000015");

        private static readonly Guid Paleta1_Id = new Guid("20000000-0000-0000-0000-000000000001");
        private static readonly Guid Paleta2_Id = new Guid("20000000-0000-0000-0000-000000000002");
        private static readonly Guid Paleta3_Id = new Guid("20000000-0000-0000-0000-000000000003");

        private static readonly Guid Faktura1_Id = new Guid("30000000-0000-0000-0000-000000000001");
        private static readonly Guid Faktura2_Id = new Guid("30000000-0000-0000-0000-000000000002");
        private static readonly Guid Faktura3_Id = new Guid("30000000-0000-0000-0000-000000000003");

        private static readonly DateTime DatumSeedovanja = new DateTime(2026, 3, 1, 9, 30, 0);

        private static string Serija(Guid idVina) => $"VN-2025-{idVina}";

        public static void UbaciInicijalnePodatke(
            IKorisniciRepozitorijum korisniciRepo,
            IVinovaLozaRepozitorijum vinoveLozeRepo,
            IVinoRepozitorijum vinoRepo,
            IPaleteRepozitorijum paleteRepo,
            IFaktureRepozitorijum faktureRepo,
            IVinskiPodrumRepozitorijum vinskiPodrumRepo)
        {
            SeedKorisnikeAkoNema(korisniciRepo);
            SeedPodrumeAkoNema(vinskiPodrumRepo);
            SeedLozeAkoNema(vinoveLozeRepo);
            SeedVinaAkoNema(vinoRepo);
            SeedPaleteAkoNema(paleteRepo);
            SeedFaktureAkoNema(faktureRepo);
        }

        private static void SeedKorisnikeAkoNema(IKorisniciRepozitorijum repo)
        {
            if (repo == null) return;

            try { if (repo.SviKorisnici().Any()) return; }
            catch { }

            repo.DodajKorisnika(new Korisnik("enolog", "Enolog2026", "Glavni Enolog", TipKorisnika.GlavniEnolog));
            repo.DodajKorisnika(new Korisnik("kelar", "Kelar2026", "Kelar Podruma", TipKorisnika.KelarMajstor));
        }

        private static void SeedPodrumeAkoNema(IVinskiPodrumRepozitorijum repo)
        {
            if (repo == null) return;

            try
            {
                if (repo.SviVinskiPodrumi().Any(p => p.Id == PodrumA_Id || p.Id == PodrumB_Id || p.Id == PodrumC_Id))
                    return;
            }
            catch { }

            repo.DodajVinskiPodrum(new VinskiPodrum(PodrumA_Id, "Vinarija Aleksandrovic", 12.0, 250));
            repo.DodajVinskiPodrum(new VinskiPodrum(PodrumB_Id, "Vinarija Kovacevic", 13.5, 180));
            repo.DodajVinskiPodrum(new VinskiPodrum(PodrumC_Id, "Vinarija Plantaže", 12.8, 320));
        }

        private static void SeedLozeAkoNema(IVinovaLozaRepozitorijum repo)
        {
            if (repo == null) return;

            var lozeZaSeed = new List<VinovaLoza>
            {
                new VinovaLoza(Loza01_Id, "Prokupac", 23.5, 2018, "Toplicki vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
                new VinovaLoza(Loza02_Id, "Tamjanika", 22.1, 2019, "Zupski vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
                new VinovaLoza(Loza03_Id, "Vranac", 24.0, 2017, "Crnogorski vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
                new VinovaLoza(Loza04_Id, "Chardonnay", 21.7, 2020, "Sremski vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
                new VinovaLoza(Loza05_Id, "Cabernet Sauvignon", 24.2, 2016, "Sumadijski vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
                new VinovaLoza(Loza06_Id, "Merlot", 23.2, 2016, "Sremski vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
                new VinovaLoza(Loza07_Id, "Pinot Noir", 22.0, 2021, "Fruškogorski vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
                new VinovaLoza(Loza08_Id, "Sauvignon Blanc", 21.5, 2020, "Banatski vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
                new VinovaLoza(Loza09_Id, "Riesling", 21.0, 2019, "Fruškogorski vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
                new VinovaLoza(Loza10_Id, "Syrah", 24.5, 2018, "Sumadijski vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
                new VinovaLoza(Loza11_Id, "Malbec", 24.3, 2017, "Zupski vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
                new VinovaLoza(Loza12_Id, "Graševina", 20.9, 2020, "Sremski vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
                new VinovaLoza(Loza13_Id, "Žilavka", 21.2, 2019, "Hercegovacki vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
                new VinovaLoza(Loza14_Id, "Muscat", 20.8, 2021, "Zupski vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
                new VinovaLoza(Loza15_Id, "Rose blend", 22.3, 2022, "Sumadijski vinogradarski reon", FazaZrelostiLoze.SpremnaZaBerbu),
            };

            try
            {
                var spremne = repo.PronadjiVinoveLozePoFazi(FazaZrelostiLoze.SpremnaZaBerbu);
                if (spremne != null && spremne.Any(l => lozeZaSeed.Any(z => z.Id == l.Id)))
                    return;
            }
            catch { }

            foreach (var loza in lozeZaSeed)
                repo.DodajVinovuLozu(loza);
        }

        private static void SeedVinaAkoNema(IVinoRepozitorijum repo)
        {
            if (repo == null) return;

            var vinaZaSeed = new List<Vino>
            {
                new Vino(Vino01_Id, "Prokupac Reserve", KategorijaVina.Premium, 0.75, Serija(Vino01_Id), Loza01_Id, DatumSeedovanja),
                new Vino(Vino02_Id, "Vranac Barrique", KategorijaVina.Premium, 0.75, Serija(Vino02_Id), Loza03_Id, DatumSeedovanja),
                new Vino(Vino03_Id, "Cabernet Sauvignon Grand", KategorijaVina.Premium, 0.75, Serija(Vino03_Id), Loza05_Id, DatumSeedovanja),
                new Vino(Vino04_Id, "Chardonnay Reserve", KategorijaVina.Premium, 0.75, Serija(Vino04_Id), Loza04_Id, DatumSeedovanja),
                new Vino(Vino05_Id, "Syrah Reserve", KategorijaVina.Premium, 0.75, Serija(Vino05_Id), Loza10_Id, DatumSeedovanja),

                new Vino(Vino06_Id, "Tamjanika Classic", KategorijaVina.Kvalitetno, 0.75, Serija(Vino06_Id), Loza02_Id, DatumSeedovanja),
                new Vino(Vino07_Id, "Merlot Classic", KategorijaVina.Kvalitetno, 0.75, Serija(Vino07_Id), Loza06_Id, DatumSeedovanja),
                new Vino(Vino08_Id, "Pinot Noir Classic", KategorijaVina.Kvalitetno, 0.75, Serija(Vino08_Id), Loza07_Id, DatumSeedovanja),
                new Vino(Vino09_Id, "Sauvignon Blanc Classic", KategorijaVina.Kvalitetno, 0.75, Serija(Vino09_Id), Loza08_Id, DatumSeedovanja),
                new Vino(Vino10_Id, "Riesling Classic", KategorijaVina.Kvalitetno, 0.75, Serija(Vino10_Id), Loza09_Id, DatumSeedovanja),

                new Vino(Vino11_Id, "Prokupac Stolno", KategorijaVina.Stolno, 1.5, Serija(Vino11_Id), Loza01_Id, DatumSeedovanja),
                new Vino(Vino12_Id, "Graševina Stolno", KategorijaVina.Stolno, 1.5, Serija(Vino12_Id), Loza12_Id, DatumSeedovanja),
                new Vino(Vino13_Id, "Žilavka Stolno", KategorijaVina.Stolno, 1.5, Serija(Vino13_Id), Loza13_Id, DatumSeedovanja),
                new Vino(Vino14_Id, "Muscat Stolno", KategorijaVina.Stolno, 1.5, Serija(Vino14_Id), Loza14_Id, DatumSeedovanja),
                new Vino(Vino15_Id, "Rose Stolno", KategorijaVina.Stolno, 1.5, Serija(Vino15_Id), Loza15_Id, DatumSeedovanja),
            };

            try
            {
                var premium = repo.PronadjiVinaPoKategoriji(KategorijaVina.Premium) ?? Enumerable.Empty<Vino>();
                var kvalitetno = repo.PronadjiVinaPoKategoriji(KategorijaVina.Kvalitetno) ?? Enumerable.Empty<Vino>();
                var stolno = repo.PronadjiVinaPoKategoriji(KategorijaVina.Stolno) ?? Enumerable.Empty<Vino>();

                var sva = premium.Concat(kvalitetno).Concat(stolno).ToList();
                if (sva.Any(v => vinaZaSeed.Any(z => z.Id == v.Id)))
                    return;
            }
            catch { }

            foreach (var vino in vinaZaSeed)
                repo.DodajVino(vino);
        }

        private static void SeedPaleteAkoNema(IPaleteRepozitorijum repo)
        {
            if (repo == null) return;

            var paleteZaSeed = new List<Paleta>
            {
                new Paleta(
                    Paleta1_Id,
                    "PAL-NS-001",
                    "Novi Sad",
                    PodrumA_Id,
                    new List<Guid> { Vino01_Id, Vino02_Id, Vino06_Id, Vino09_Id, Vino11_Id },
                    StatusPalete.Upakovana
                ),
                new Paleta(
                    Paleta2_Id,
                    "PAL-BG-002",
                    "Beograd",
                    PodrumB_Id,
                    new List<Guid> { Vino03_Id, Vino04_Id, Vino07_Id, Vino08_Id, Vino12_Id },
                    StatusPalete.Otpremljena
                ),
                new Paleta(
                    Paleta3_Id,
                    "PAL-NI-003",
                    "Niš",
                    PodrumC_Id,
                    new List<Guid> { Vino05_Id, Vino10_Id, Vino13_Id, Vino14_Id, Vino15_Id },
                    StatusPalete.Raspakovana
                ),
            };

            try
            {
                var sve = new List<Paleta>();
                var up = repo.PronadjiPaletePoStatusu(StatusPalete.Upakovana);
                var ot = repo.PronadjiPaletePoStatusu(StatusPalete.Otpremljena);
                var ra = repo.PronadjiPaletePoStatusu(StatusPalete.Raspakovana);

                if (up != null) sve.AddRange(up);
                if (ot != null) sve.AddRange(ot);
                if (ra != null) sve.AddRange(ra);

                if (sve.Any(p => paleteZaSeed.Any(z => z.Id == p.Id)))
                    return;
            }
            catch { }

            foreach (var paleta in paleteZaSeed)
                repo.DodajPaletu(paleta);
        }

        private static void SeedFaktureAkoNema(IFaktureRepozitorijum repo)
        {
            if (repo == null) return;

            try
            {
                var sve = new List<Faktura>();
                var r = repo.PronadjiFakturePoTipuProdaje(TipProdaje.Restoranska);
                var d = repo.PronadjiFakturePoTipuProdaje(TipProdaje.Diskont);

                if (r != null) sve.AddRange(r);
                if (d != null) sve.AddRange(d);

                if (sve.Any(f => f.Id == Faktura1_Id || f.Id == Faktura2_Id || f.Id == Faktura3_Id))
                    return;
            }
            catch { }

            var f1 = new Faktura(
                Faktura1_Id,
                DatumSeedovanja,
                TipProdaje.Restoranska,
                NacinPlacanja.Predracun,
                new List<StavkaFakture>
                {
                    new StavkaFakture(Vino01_Id, 2, 1650m),
                    new StavkaFakture(Vino06_Id, 4, 980m),
                    new StavkaFakture(Vino09_Id, 3, 1050m),
                }
            );

            var f2 = new Faktura(
                Faktura2_Id,
                DatumSeedovanja.AddDays(2),
                TipProdaje.Diskont,
                NacinPlacanja.Gotovina,
                new List<StavkaFakture>
                {
                    new StavkaFakture(Vino11_Id, 10, 520m),
                    new StavkaFakture(Vino12_Id, 8, 490m),
                    new StavkaFakture(Vino15_Id, 6, 560m),
                }
            );

            var f3 = new Faktura(
                Faktura3_Id,
                DatumSeedovanja.AddDays(5),
                TipProdaje.Restoranska,
                NacinPlacanja.Predracun,
                new List<StavkaFakture>
                {
                    new StavkaFakture(Vino02_Id, 2, 1750m),
                    new StavkaFakture(Vino04_Id, 2, 1850m),
                    new StavkaFakture(Vino07_Id, 1, 1250m),
                }
            );

            repo.DodajFakturu(f1);
            repo.DodajFakturu(f2);
            repo.DodajFakturu(f3);
        }
    }
}
