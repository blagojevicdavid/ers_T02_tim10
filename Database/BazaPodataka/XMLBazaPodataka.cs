using Domain.BazaPodataka;
using System.IO;
using System.Xml.Serialization;

namespace Database.BazaPodataka
{
    public class XmlBazaPodataka : IBazaPodataka
    {
        private readonly string putanjaDoFajla;

        public TabeleBazaPodataka Tabele { get; set; }

        public XmlBazaPodataka() : this("podaci.xml")
        {
        }

        public XmlBazaPodataka(string putanja)
        {
            putanjaDoFajla = string.IsNullOrWhiteSpace(putanja) ? "podaci.xml" : putanja;
            Tabele = UcitajIliKreirajPrazno();
        }

        public bool SacuvajPromene()
        {
            try
            {
                var folder = Path.GetDirectoryName(putanjaDoFajla);
                if (!string.IsNullOrWhiteSpace(folder) && !Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                using FileStream fs = new FileStream(putanjaDoFajla, FileMode.Create, FileAccess.Write, FileShare.None);
                XmlSerializer serializer = new XmlSerializer(typeof(TabeleBazaPodataka));
                serializer.Serialize(fs, Tabele);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private TabeleBazaPodataka UcitajIliKreirajPrazno()
        {
            try
            {
                if (!File.Exists(putanjaDoFajla))
                    return new TabeleBazaPodataka();

                using FileStream fs = new FileStream(putanjaDoFajla, FileMode.Open, FileAccess.Read, FileShare.Read);
                XmlSerializer serializer = new XmlSerializer(typeof(TabeleBazaPodataka));

                if (serializer.Deserialize(fs) is TabeleBazaPodataka ucitano && ucitano != null)
                    return ucitano;

                return new TabeleBazaPodataka();
            }
            catch
            {
                return new TabeleBazaPodataka();
            }
        }
    }
}
