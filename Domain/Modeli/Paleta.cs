using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Enumeracije;

namespace Domain.Modeli
{
    public class Paleta
    {
        public Guid Id { get; set; }
        public string Sifra { get; set; } = string.Empty;
        public string AdresaOdredista { get; set; } = string.Empty;
        public Guid VinskiPodrumId { get; set; }
        public List<Guid> VinaIds { get; set; } = new();
        public StatusPalete Status { get; set; }

        
        public Paleta()
        {
            Id = Guid.NewGuid();
            Status = StatusPalete.Upakovana;
        }

        public Paleta(Guid id, string sifra, string adresaOdredista, Guid vinskiPodrumId, List<Guid> vinaIds, StatusPalete status)
        {
            Id = id;
            Sifra = sifra;
            AdresaOdredista = adresaOdredista;
            VinskiPodrumId = vinskiPodrumId;
            VinaIds = vinaIds ?? new List<Guid>();
            Status = status;
        }
    }
}

