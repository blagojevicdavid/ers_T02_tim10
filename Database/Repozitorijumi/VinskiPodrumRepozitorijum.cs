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
    public class VinskiPodrumRepozitorijum : IVinskiPodrumRepozitorijum
    {
        private readonly IBazaPodataka bazaPodataka;

        public VinskiPodrumRepozitorijum(IBazaPodataka baza)
        {
            bazaPodataka = baza;
        }

        public bool AzurirajVinskiPodrum(VinskiPodrum podrum)
        {
            try
            {
                for (int i = 0; i < bazaPodataka.Tabele.VinskiPodrumi.Count; i++)
                {
                    if (bazaPodataka.Tabele.VinskiPodrumi[i].Id == podrum.Id)
                    {
                        bazaPodataka.Tabele.VinskiPodrumi[i] = podrum;
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

        public VinskiPodrum DodajVinskiPodrum(VinskiPodrum podrum)
        {
            try
            {
                bazaPodataka.Tabele.VinskiPodrumi.Add(podrum);
                bool uspesno = bazaPodataka.SacuvajPromene();

                if (uspesno)
                    return podrum;
                else
                    return new VinskiPodrum();
            }
            catch
            {
                return new VinskiPodrum();
            }
        }

        public VinskiPodrum PronadjiVinskiPodrumPoId(Guid id)
        {
            try
            {
                foreach (var podrum in bazaPodataka.Tabele.VinskiPodrumi)
                {
                    if (podrum.Id == id)
                        return podrum;
                }

                return new VinskiPodrum();
            }
            catch
            {
                return new VinskiPodrum();
            }
        }

        public IEnumerable<VinskiPodrum> SviVinskiPodrumi()
        {
            try
            {
                return bazaPodataka.Tabele.VinskiPodrumi;
            }
            catch
            {
                return [];
            }
        }
    }
}
