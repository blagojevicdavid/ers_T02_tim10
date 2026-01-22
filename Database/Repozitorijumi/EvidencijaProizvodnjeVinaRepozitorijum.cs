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
    public class EvidencijaProizvodnjeVinaRepozitorijum : IEvidencijaProizvodnjeVinaRepozitorijum
    {
        private readonly IBazaPodataka bazaPodataka;

        public EvidencijaProizvodnjeVinaRepozitorijum(IBazaPodataka bazaPodataka)
        {
            this.bazaPodataka = bazaPodataka;
        }
        public EvidencijaProizvodnjeVina DodajEvidenciju(EvidencijaProizvodnjeVina evidencija)
        {
            bazaPodataka.Tabele.EvidencijeProizvodnjeVina.Add(evidencija);
            bazaPodataka.SacuvajPromene();
            return evidencija;
        }
        public IEnumerable<EvidencijaProizvodnjeVina> SveEvidencije()
        {
            return bazaPodataka.Tabele.EvidencijeProizvodnjeVina;
        }

        public IEnumerable<EvidencijaProizvodnjeVina> EvidencijeZaFermentaciju(Guid fermentacijaId)
        {
            return bazaPodataka.Tabele.EvidencijeProizvodnjeVina
                .Where(e => e.FermentacijaId == fermentacijaId)
                .OrderBy(e => e.DatumVreme);
        }
    }
}
