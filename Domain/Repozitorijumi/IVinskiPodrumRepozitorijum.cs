using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface IVinskiPodrumRepozitorijum
    {
        bool AzurirajVinskiPodrum(VinskiPodrum podrum);

        VinskiPodrum DodajVinskiPodrum(VinskiPodrum podrum);

        VinskiPodrum PronadjiVinskiPodrumPoId(Guid id);

        IEnumerable<VinskiPodrum> SviVinskiPodrumi();
    }
}

