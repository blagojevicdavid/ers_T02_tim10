using Domain.BazaPodataka;

namespace Database.BazaPodataka
{
    public class XMLBazaPodataka : IBazaPodataka
    {
        public TabeleBazePodataka Tabele { get; set; }

        public XMLBazaPodataka()
        {
            Tabele = new TabeleBazePodataka();
        }

    }
}
