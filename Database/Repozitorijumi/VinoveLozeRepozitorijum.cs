using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class VinoveLozeRepozitorijum : IVinovaLozaRepozitorijum
    {
        private readonly IBazaPodataka bazaPodataka;

        public VinoveLozeRepozitorijum(IBazaPodataka baza)
        {
            bazaPodataka = baza;
        }

        public bool AzurirajVinovuLozu(VinovaLoza loza)
        {
            try
            {
                for (int i = 0; i < bazaPodataka.Tabele.VinoveLoze.Count; i++)
                {
                    if (bazaPodataka.Tabele.VinoveLoze[i].Id == loza.Id)
                    {
                        bazaPodataka.Tabele.VinoveLoze[i] = loza;
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

        public VinovaLoza DodajVinovuLozu(VinovaLoza loza)
        {
            try
            {


                bazaPodataka.Tabele.VinoveLoze.Add(loza);
                bool uspesno = bazaPodataka.SacuvajPromene();

                if (uspesno)
                    return loza;
                else
                    return new VinovaLoza();
            }
            catch
            {
                return new VinovaLoza();
            }
        }

        public VinovaLoza PronadjiVinovuLozuPoId(Guid id)
        {
            try
            {
                foreach (var loza in bazaPodataka.Tabele.VinoveLoze)
                {
                    if (loza.Id == id)
                        return loza;
                }

                return new VinovaLoza();
            }
            catch
            {
                return new VinovaLoza();
            }
        }

        public IEnumerable<VinovaLoza> PronadjiVinoveLozePoFazi(FazaZrelostiLoze faza)
        {
            try
            {
                List<VinovaLoza> rezultat = [];
                foreach (var loza in bazaPodataka.Tabele.VinoveLoze)
                {
                    if (loza.FazaZrelosti == faza)
                        rezultat.Add(loza);
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
