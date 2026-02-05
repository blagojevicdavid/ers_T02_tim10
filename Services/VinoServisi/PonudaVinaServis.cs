using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            foreach (KategorijaVina kat in System.Enum.GetValues(typeof(KategorijaVina)))
            {
                sve.AddRange(vinoRepo.PronadjiVinaPoKategoriji(kat));
            }

            // ukloni duplikate 
            return sve
                .GroupBy(v => v.Id)
                .Select(g => g.First())
                .OrderBy(v => v.Naziv)
                .ToList();
        }

        public Vino? PronadjiPoSifri(string sifra)
        {
            return VratiPonudu().FirstOrDefault(v => v.Sifra == sifra);
        }
    }
}
