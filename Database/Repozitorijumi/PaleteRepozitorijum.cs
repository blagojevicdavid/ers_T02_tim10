using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class PaleteRepozitorijum : IPaleteRepozitorijum
    {
        private readonly IBazaPodataka bazaPodataka;

        public PaleteRepozitorijum(IBazaPodataka baza)
        {
            bazaPodataka = baza;
        }

        public bool AzurirajPaletu(Paleta paleta)
        {
            try
            {
                for (int i = 0; i < bazaPodataka.Tabele.Palete.Count; i++)
                {
                    if (bazaPodataka.Tabele.Palete[i].Id == paleta.Id)
                    {
                        bazaPodataka.Tabele.Palete[i] = paleta;
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

        public Paleta DodajPaletu(Paleta paleta)
        {
            try
            {
                
                paleta.Sifra = $"PL-2025-{paleta.Id}";

                bazaPodataka.Tabele.Palete.Add(paleta);
                bool uspesno = bazaPodataka.SacuvajPromene();

                if (uspesno)
                    return paleta;
                else
                    return new Paleta();
            }
            catch
            {
                return new Paleta();
            }
        }

        public Paleta PronadjiPaletuPoId(Guid id)
        {
            try
            {
                foreach (var paleta in bazaPodataka.Tabele.Palete)
                {
                    if (paleta.Id == id)
                        return paleta;
                }

                return new Paleta();
            }
            catch
            {
                return new Paleta();
            }
        }

        public IEnumerable<Paleta> PronadjiPaletePoStatusu(StatusPalete status)
        {
            try
            {
                List<Paleta> rezultat = [];
                foreach (var paleta in bazaPodataka.Tabele.Palete)
                {
                    if (paleta.Status == status)
                        rezultat.Add(paleta);
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

