using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (kolicinaKg <= 0)
            {
                _logger.Evidentiraj(TipEvidencije.WARNING, "Količina berbe mora biti veća od 0.");
                return new BerbaLoze();
            }

            if (datumBerbe > DateTime.Now)
            {
                _logger.Evidentiraj(TipEvidencije.WARNING, "Datum berbe ne može biti u budućnosti.");
                return new BerbaLoze();
            }

            var berba = new BerbaLoze(Guid.NewGuid(), datumBerbe, kolicinaKg);

            var sacuvana = _repo.Dodaj(berba);
            if (sacuvana.Id == Guid.Empty)
            {
                _logger.Evidentiraj(TipEvidencije.ERROR, "Neuspešno čuvanje berbe.");
                return new BerbaLoze();
            }

            return sacuvana;
        }

        public IEnumerable<BerbaLoze> VratiSveBerbe()
        {
            return _repo.Sve();
        }

        public BerbaLoze Pronadji(Guid id)
        {
            return _repo.PronadjiPoId(id);
        }

        public bool Azuriraj(Guid id, DateTime datumBerbe, double kolicinaKg)
        {
            if (kolicinaKg <= 0) return false;
            if (datumBerbe > DateTime.Now) return false;

            var postojeca = _repo.PronadjiPoId(id);
            if (postojeca.Id == Guid.Empty) return false;

            postojeca.DatumBerbe = datumBerbe;
            postojeca.KolicinaKg = kolicinaKg;

            return _repo.Azuriraj(postojeca);
        }
    }
}