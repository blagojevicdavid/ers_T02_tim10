using Domain.Modeli;

namespace Domain.BazaPodataka
{
    public class TabeleBazaPodataka
    {
        public List<Korisnik> Korisnici { get; set; } = [];
        // TODO: Add other database tables as needed

        public List<VinovaLoza> VinoveLoze { get; set; } = new();

        public List<Vino> Vina { get; set; } = new();

        public List<Paleta> Palete { get; set; } = new();

        public List<VinskiPodrum> VinskiPodrumi { get; set; } = new();

        public List<Faktura> Fakture { get; set; } = new();




        public TabeleBazaPodataka() { }
    }
}
