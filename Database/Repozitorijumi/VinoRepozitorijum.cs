using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Database.Repozitorijumi
{
    public class VinoRepozitorijum : IVinoRepozitorijum
    {
        private readonly IBazaPodataka bazaPodataka;

        public VinoRepozitorijum(IBazaPodataka baza)
        {
            bazaPodataka = baza;
        }

        public bool AzurirajVino(Vino vino)
        {
            try
            {
                for (int i = 0; i < bazaPodataka.Tabele.Vina.Count; i++)
                {
                    if (bazaPodataka.Tabele.Vina[i].Id == vino.Id)
                    {
                        bazaPodataka.Tabele.Vina[i] = vino;
                        return bazaPodataka.SacuvajPromene();
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public Vino DodajVino(Vino vino)
        {
            try
            {
                // Šifra po specifikaciji: VN-2025-ID_VINA
                vino.Sifra = $"VN-2025-{vino.Id}";

                bazaPodataka.Tabele.Vina.Add(vino);
                bool uspesno = bazaPodataka.SacuvajPromene();

                if (uspesno)
                    return vino;
                else
                    return new Vino();
            }
            catch
            {
                return new Vino();
            }
        }

   
            public bool PronadjiVinoPoId(Guid id, out Vino vino)
        {
            vino = new Vino(); 

            try
            {
                foreach (var v in bazaPodataka.Tabele.Vina)
                {
                    if (v.Id == id)
                    {
                        vino = v;
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        

        public IEnumerable<Vino> PronadjiVinaPoKategoriji(KategorijaVina kategorija)
        {
            try
            {
                List<Vino> rezultat = [];
                foreach (var vino in bazaPodataka.Tabele.Vina)
                {
                    if (vino.Kategorija == kategorija)
                        rezultat.Add(vino);
                }
                return rezultat;
            }
            catch
            {
                return [];
            }
        }
    }
}
