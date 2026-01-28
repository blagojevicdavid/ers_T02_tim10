using Domain.Modeli;

namespace Domain.BazaPodataka
{
    public class TabeleBazaPodataka
    {
        public List<Korisnik> Korisnici { get; set; } = [];
        

        public List<VinovaLoza> VinoveLoze { get; set; } = new();

        public List<Vino> Vina { get; set; } = new();

        public List<Paleta> Palete { get; set; } = new();

        public List<VinskiPodrum> VinskiPodrumi { get; set; } = new();

        public List<Faktura> Fakture { get; set; } = new();

        public List<Fermentacija> Fermentacije { get; set; } = new();

        public List<MerenjeSecera> MerenjaSecera { get; set; } = new();

        public List<EvidencijaProizvodnjeVina> EvidencijeProizvodnjeVina { get; set; } = new();




        public TabeleBazaPodataka() { }
    }
}
