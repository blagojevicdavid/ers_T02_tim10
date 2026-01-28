using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Database.BazaPodataka
{
    public static class PocetniPodaci
    {
        private static readonly Guid PodrumA_Id = new Guid("aa111111-bbbb-4444-8888-000000000001");
        private static readonly Guid PodrumB_Id = new Guid("bb222222-cccc-5555-9999-000000000002");

        private static readonly Guid LozaA_Id = new Guid("11111111-aaaa-4444-8888-000000000010");
        private static readonly Guid LozaB_Id = new Guid("22222222-bbbb-5555-9999-000000000020");

        private static readonly Guid Vino1_Id = new Guid("33333333-cccc-6666-aaaa-000000000030");
        private static readonly Guid Vino2_Id = new Guid("44444444-dddd-7777-bbbb-000000000040");
        private static readonly Guid Vino3_Id = new Guid("55555555-eeee-8888-cccc-000000000050");

        private static readonly Guid Paleta1_Id = new Guid("66666666-ffff-9999-dddd-000000000060");
        private static readonly Guid Faktura1_Id = new Guid("77777777-1111-2222-3333-000000000070");

        private static readonly DateTime DatumSeedovanja = new DateTime(2026, 3, 1, 9, 30, 0);

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
            SeedPaletuAkoNema(paleteRepo);
            SeedFakturuAkoNema(faktureRepo);
        }

        private static void SeedKorisnikeAkoNema(IKorisniciRepozitorijum repo)
        {
            if (repo == null) return;

            try
            {
                if (repo.SviKorisnici().Any())
                    return;
            }
            catch
            {
            }

            repo.DodajKorisnika(new Korisnik("enolog", "Enolog2026", "Glavni Enolog", TipKorisnika.GlavniEnolog));
            repo.DodajKorisnika(new Korisnik("kelar", "Kelar2026", "Kelar Podruma", TipKorisnika.KelarMajstor));
        }

        private static void SeedPodrumeAkoNema(IVinskiPodrumRepozitorijum repo)
        {
            if (repo == null) return;

            try
            {
                if (repo.SviVinskiPodrumi().Any(p => p.Id == PodrumA_Id || p.Id == PodrumB_Id))
                    return;
            }
            catch
            {
            }

            repo.DodajVinskiPodrum(new VinskiPodrum(PodrumA_Id, "Vinarija Aleksandrovic", 12.0, 250));
            repo.DodajVinskiPodrum(new VinskiPodrum(PodrumB_Id, "Vinarija Kovacevic", 13.5, 180));
        }

        private static void SeedLozeAkoNema(IVinovaLozaRepozitorijum repo)
        {
            if (repo == null) return;

            try
            {
                var spremne = repo.PronadjiVinoveLozePoFazi(FazaZrelostiLoze.SpremnaZaBerbu);
                if (spremne != null && spremne.Any(l => l.Id == LozaA_Id || l.Id == LozaB_Id))
                    return;
            }
            catch
            {
            }

            repo.DodajVinovuLozu(new VinovaLoza(LozaA_Id, "Prokupac", 23.5, 2018, "Toplicki vinogradarski rejon", FazaZrelostiLoze.SpremnaZaBerbu));
            repo.DodajVinovuLozu(new VinovaLoza(LozaB_Id, "Tamjanika", 22.1, 2019, "Zupski vinogradarski rejon", FazaZrelostiLoze.SpremnaZaBerbu));
        }

        private static void SeedVinaAkoNema(IVinoRepozitorijum repo)
        {
            if (repo == null) return;

            try
            {
                var premium = repo.PronadjiVinaPoKategoriji(KategorijaVina.Premium);
                var kvalitetno = repo.PronadjiVinaPoKategoriji(KategorijaVina.Kvalitetno);
                var stolno = repo.PronadjiVinaPoKategoriji(KategorijaVina.Stolno);

                bool postoji =
                    (premium != null && premium.Any(v => v.Id == Vino1_Id)) ||
                    (kvalitetno != null && kvalitetno.Any(v => v.Id == Vino2_Id)) ||
                    (stolno != null && stolno.Any(v => v.Id == Vino3_Id));

                if (postoji) return;
            }
            catch
            {
            }

            repo.DodajVino(new Vino(Vino1_Id, "Prokupac Reserve", KategorijaVina.Premium, 0.75, "PRK-RES-001", LozaA_Id, DatumSeedovanja));
            repo.DodajVino(new Vino(Vino2_Id, "Tamjanika Classic", KategorijaVina.Kvalitetno, 0.75, "TMJ-CLS-002", LozaB_Id, DatumSeedovanja));
            repo.DodajVino(new Vino(Vino3_Id, "Prokupac Stolno", KategorijaVina.Stolno, 1.5, "PRK-STL-003", LozaA_Id, DatumSeedovanja));
        }

        private static void SeedPaletuAkoNema(IPaleteRepozitorijum repo)
        {
            if (repo == null) return;

            try
            {
                var sve = new List<Paleta>();
                var up = repo.PronadjiPaletePoStatusu(StatusPalete.Upakovana);
                var ot = repo.PronadjiPaletePoStatusu(StatusPalete.Otpremljena);
                var ra = repo.PronadjiPaletePoStatusu(StatusPalete.Raspakovana);

                if (up != null) sve.AddRange(up);
                if (ot != null) sve.AddRange(ot);
                if (ra != null) sve.AddRange(ra);

                if (sve.Any(p => p.Id == Paleta1_Id))
                    return;
            }
            catch
            {
            }

            var vinoIds = new List<Guid> { Vino1_Id, Vino2_Id, Vino3_Id };

            repo.DodajPaletu(new Paleta(
                Paleta1_Id,
                "PAL-NS-001",
                "Novi Sad",
                PodrumA_Id,
                vinoIds,
                StatusPalete.Upakovana
            ));
        }

        private static void SeedFakturuAkoNema(IFaktureRepozitorijum repo)
        {
            if (repo == null) return;

            try
            {
                var sve = new List<Faktura>();
                var r = repo.PronadjiFakturePoTipuProdaje(TipProdaje.Restoranska);
                var d = repo.PronadjiFakturePoTipuProdaje(TipProdaje.Diskont);

                if (r != null) sve.AddRange(r);
                if (d != null) sve.AddRange(d);

                if (sve.Any(f => f.Id == Faktura1_Id))
                    return;
            }
            catch
            {
            }

            var stavke = new List<StavkaFakture>
            {
                new StavkaFakture(Vino1_Id, 2, 1350m),
                new StavkaFakture(Vino2_Id, 1, 980m),
                new StavkaFakture(Vino3_Id, 3, 720m),
            };

            var f = new Faktura(
                Faktura1_Id,
                DatumSeedovanja,
                TipProdaje.Restoranska,
                NacinPlacanja.Kartica,
                stavke
            );

            repo.DodajFakturu(f);
        }
    }
}
