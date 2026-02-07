using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;
using System.Collections.Generic;

namespace Services.VinogradServisi
{
    public class BerbaLozeServis : IBerbaLozeServis
    {
        private readonly IBerbaLozeRepozitorijum _repo;
        private readonly ILoggerServis _logger;

        public BerbaLozeServis(IBerbaLozeRepozitorijum repo, ILoggerServis logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public BerbaLoze EvidentirajBerbu(DateTime datumBerbe, double kolicinaKg)
        {
            if (_repo == null || _logger == null) return new BerbaLoze();

            if (kolicinaKg <= 0)
            {
                _logger.Evidentiraj(TipEvidencije.WARNING, "Količina berbe mora biti veća od 0.");
                return new BerbaLoze();
            }

            DateTime now = DateTime.Now;
            if (datumBerbe > now)
            {
                _logger.Evidentiraj(TipEvidencije.WARNING, "Datum berbe ne može biti u budućnosti.");
                return new BerbaLoze();
            }

            var berba = new BerbaLoze(Guid.NewGuid(), datumBerbe, kolicinaKg);

            var sacuvana = _repo.Dodaj(berba);
            if (sacuvana == null || sacuvana.Id == Guid.Empty)
            {
                _logger.Evidentiraj(TipEvidencije.ERROR, "Neuspešno čuvanje berbe.");
                return new BerbaLoze();
            }

            return sacuvana;
        }

        public IEnumerable<BerbaLoze> VratiSveBerbe()
        {
            if (_repo == null) return new List<BerbaLoze>();

            var sve = _repo.Sve();
            if (sve == null) return new List<BerbaLoze>();

            return sve;
        }

        public BerbaLoze Pronadji(Guid id)
        {
            if (_repo == null) return new BerbaLoze();
            if (id == Guid.Empty) return new BerbaLoze();

            var b = _repo.PronadjiPoId(id);
            if (b == null) return new BerbaLoze();

            return b;
        }

        public bool Azuriraj(Guid id, DateTime datumBerbe, double kolicinaKg)
        {
            if (_repo == null) return false;

            if (id == Guid.Empty) return false;
            if (kolicinaKg <= 0) return false;

            DateTime now = DateTime.Now;
            if (datumBerbe > now) return false;

            var postojeca = _repo.PronadjiPoId(id);
            if (postojeca == null || postojeca.Id == Guid.Empty) return false;

            postojeca.DatumBerbe = datumBerbe;
            postojeca.KolicinaKg = kolicinaKg;

            return _repo.Azuriraj(postojeca);
        }
    }
}
