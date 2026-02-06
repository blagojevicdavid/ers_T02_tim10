using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.VinoServisi
{
    public class PonudaVinaServis : IPonudaVinaServis
    {
        private readonly IVinoRepozitorijum vinoRepo;

        public PonudaVinaServis(IVinoRepozitorijum vinoRepo)
        {
            this.vinoRepo = vinoRepo;
        }

        public List<Vino> VratiPonudu()
        {
            var sve = new List<Vino>();

            foreach (KategorijaVina kat in Enum.GetValues(typeof(KategorijaVina)))
            {
                var lista = vinoRepo.PronadjiVinaPoKategoriji(kat) ?? new List<Vino>();
                sve.AddRange(lista);
            }

            
            return sve
                .Where(v => v != null)
                .GroupBy(v => v.Id)
                .Select(g => g.First())
                .OrderBy(v => v.Naziv)
                .ToList();
        }

        public Vino? PronadjiPoSifri(string sifra)
        {
            if (string.IsNullOrWhiteSpace(sifra))
                return null;

            sifra = sifra.Trim();

            //
            return VratiPonudu()
                .FirstOrDefault(v => !string.IsNullOrWhiteSpace(v.Sifra) &&
                                     string.Equals(v.Sifra.Trim(), sifra, StringComparison.OrdinalIgnoreCase));
        }
    }
}
