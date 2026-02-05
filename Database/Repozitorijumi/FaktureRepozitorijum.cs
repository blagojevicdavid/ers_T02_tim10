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
    public class FaktureRepozitorijum : IFaktureRepozitorijum
    {
        private readonly IBazaPodataka bazaPodataka;

        public FaktureRepozitorijum(IBazaPodataka baza)
        {
            bazaPodataka = baza;
        }

        public bool AzurirajFakturu(Faktura faktura)
        {
            try
            {
                for (int i = 0; i < bazaPodataka.Tabele.Fakture.Count; i++)
                {
                    if (bazaPodataka.Tabele.Fakture[i].Id == faktura.Id)
                    {
                        bazaPodataka.Tabele.Fakture[i] = faktura;
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

        public Faktura DodajFakturu(Faktura faktura)
        {
            try
            {
                bazaPodataka.Tabele.Fakture.Add(faktura);
                bool uspesno = bazaPodataka.SacuvajPromene();
                return uspesno ? faktura : new Faktura();
            }
            catch
            {
                return new Faktura();
            }
        }

        public Faktura PronadjiFakturuPoId(Guid id)
        {
            try
            {
                foreach (var f in bazaPodataka.Tabele.Fakture)
                {
                    if (f.Id == id)
                        return f;
                }
                return new Faktura();
            }
            catch
            {
                return new Faktura();
            }
        }

        public IEnumerable<Faktura> PronadjiFakturePoTipuProdaje(TipProdaje tipProdaje)
        {
            try
            {
                List<Faktura> rezultat = [];
                foreach (var f in bazaPodataka.Tabele.Fakture)
                {
                    if (f.TipProdaje == tipProdaje)
                        rezultat.Add(f);
                }
                return rezultat;
            }
            catch
            {
                return [];
            }
        }
        public IEnumerable<Faktura> PreuzmiSveFakture()
        {
            return bazaPodataka.Tabele.Fakture; // ili kako se već zove lista u bazi
        }
    }
}

