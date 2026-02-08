using System;
using System.Collections.Generic;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.ProdajaServisi.PomocneMetode
{
    internal class ProdajaPaletaFabrika
    {
        private readonly IPaleteRepozitorijum _paleteRepo;
        private readonly ILoggerServis _logger;

        public ProdajaPaletaFabrika(IPaleteRepozitorijum paleteRepo, ILoggerServis logger)
        {
            _paleteRepo = paleteRepo;
            _logger = logger;
        }

        public Paleta KreirajPaletuZaKupca(List<Guid> vinoIds, string adresaOdredista, Guid vinskiPodrumId)
        {
            if (vinoIds == null || vinoIds.Count == 0)
                return new Paleta();

            string adresa = adresaOdredista == null ? string.Empty : adresaOdredista.Trim();
            if (adresa.Length == 0 || vinskiPodrumId == Guid.Empty)
                return new Paleta();

            Paleta paleta = new Paleta
            {
                Id = Guid.NewGuid(),
                Sifra = "PL-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "-" + Guid.NewGuid().ToString().Substring(0, 8),
                AdresaOdredista = adresa,
                VinskiPodrumId = vinskiPodrumId,
                Status = StatusPalete.Upakovana,
                VinaIds = new List<Guid>(vinoIds)
            };

            var sacuvana = _paleteRepo.DodajPaletu(paleta);
            if (sacuvana == null || sacuvana.Id == Guid.Empty)
            {
                _logger.Evidentiraj(TipEvidencije.ERROR, "[PRODAJA TOK] Neuspješno čuvanje palete za kupca.");
                return new Paleta();
            }

            return sacuvana;
        }
    }
}
