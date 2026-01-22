using Domain.Enumeracije;
using Domain.Modeli;
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
        private readonly ILoggerServis _logger;
        private readonly List<BerbaLoze> _berbe = new();

        public BerbaLozeServis(ILoggerServis logger)
        {
            _logger = logger;
        }

        public BerbaLoze EvidentirajBerbu(DateTime datumBerbe, double kolicinaKg)
        {
            if (kolicinaKg <= 0)
            {
                _logger.Evidentiraj(
                    TipEvidencije.WARNING,
                    "Količina berbe mora biti veća od 0."
                );
                return null;
            }

            var berba = new BerbaLoze(Guid.NewGuid(), datumBerbe, kolicinaKg);
            _berbe.Add(berba);

            _logger.Evidentiraj(
                TipEvidencije.INFO,
                $"Evidentirana berba: {kolicinaKg} kg ({datumBerbe:dd.MM.yyyy})"
            );

            return berba;
        }

        public IEnumerable<BerbaLoze> VratiSveBerbe()
        {
            return _berbe;
        }
    }
}
