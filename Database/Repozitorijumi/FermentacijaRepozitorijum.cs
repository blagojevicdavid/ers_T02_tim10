using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class FermentacijaRepozitorijum : IFermentacijaRepozitorijum
    {
        private readonly IBazaPodataka bazaPodataka;

        public FermentacijaRepozitorijum(IBazaPodataka baza)
        {
            bazaPodataka = baza;
        }

        public Fermentacija DodajFermentaciju(Fermentacija fermentacija)
        {
            try
            {
                bazaPodataka.Tabele.Fermentacije.Add(fermentacija);
                bool uspesno = bazaPodataka.SacuvajPromene();
                return uspesno ? fermentacija : new Fermentacija();
            }
            catch
            {
                return new Fermentacija();
            }
        }

        public Fermentacija PronadjiFermentacijuPoId(Guid id)
        {
            try
            {
                foreach (Fermentacija fermentacija in bazaPodataka.Tabele.Fermentacije)
                {
                    if (fermentacija.Id == id)
                        return fermentacija;
                }
                return new Fermentacija();
            }
            catch
            {
                return new Fermentacija();
            }
        }

        public bool AzurirajFermentaciju(Fermentacija fermentacija)
        {
            try
            {
                for (int i = 0; i < bazaPodataka.Tabele.Fermentacije.Count; i++)
                {
                    if (bazaPodataka.Tabele.Fermentacije[i].Id == fermentacija.Id)
                    {
                        bazaPodataka.Tabele.Fermentacije[i] = fermentacija;
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

        public IEnumerable<Fermentacija> SveFermentacije()
        {
            try
            {
                return bazaPodataka.Tabele.Fermentacije;
            }
            catch
            {
                return [];
            }
        }
    }
}
