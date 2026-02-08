using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repozitorijumi
{
    public class BerbaLozeRepozitorijum : IBerbaLozeRepozitorijum
    {
        private readonly IBazaPodataka bazaPodataka;

        public BerbaLozeRepozitorijum(IBazaPodataka baza)
        {
            bazaPodataka = baza;
        }

        public BerbaLoze Dodaj(BerbaLoze berba)
        {
            try
            {
                bazaPodataka.Tabele.BerbeLoze.Add(berba);
                bool uspesno = bazaPodataka.SacuvajPromene();
                if (uspesno)
                    return berba;

                return new BerbaLoze();

            }
            catch
            {
                return new BerbaLoze();
            }
        }

        public IEnumerable<BerbaLoze> Sve()
        {
            return bazaPodataka.Tabele.BerbeLoze;
        }

        public BerbaLoze PronadjiPoId(Guid id)
        {
            var berba = bazaPodataka.Tabele.BerbeLoze
                .FirstOrDefault(x => x.Id == id);

            if (berba == null)
            {
                return new BerbaLoze();
            }

            return berba;
        }


        public bool Azuriraj(BerbaLoze berba)
        {
            try
            {
                var postojeca = bazaPodataka.Tabele.BerbeLoze.FirstOrDefault(x => x.Id == berba.Id);
                if (postojeca == null) return false;

                postojeca.DatumBerbe = berba.DatumBerbe;
                postojeca.KolicinaKg = berba.KolicinaKg;

                return bazaPodataka.SacuvajPromene();
            }
            catch
            {
                return false;
            }
        }
    }
}
