using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FaktureServisi
{
    public class FakturePregledServis : IFakturePregledServis
    {
        private readonly IFaktureRepozitorijum faktureRepozitorijum;

        public FakturePregledServis(IFaktureRepozitorijum faktureRepozitorijum)
        {
            this.faktureRepozitorijum = faktureRepozitorijum;
        }

        public IReadOnlyList<Faktura> PreuzmiSveFakture()
        {
            return faktureRepozitorijum
                .PreuzmiSveFakture()
                .OrderByDescending(f => f.DatumIzdavanja)
                .ToList();
        }
    }
}