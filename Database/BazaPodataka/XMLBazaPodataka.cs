using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.BazaPodataka
{
    using System.Xml.Serialization;
    using Domain.BazaPodataka;

    namespace Database.BazaPodataka
    {
        public class XmlBazaPodataka : IBazaPodataka
        {
            public TabeleBazaPodataka Tabele { get; set; }

            public XmlBazaPodataka()
            {
                // Učitavanje podataka iz XML datoteke
                try
                {
                    if (File.Exists("podaci.xml"))
                    {
                        XmlSerializer serializer =
                            new XmlSerializer(typeof(TabeleBazaPodataka));

                        StreamReader sr =
                            new StreamReader("podaci.xml");

                        object? data = serializer.Deserialize(sr);
                        sr.Close();

                        if (data != null)
                            Tabele = (TabeleBazaPodataka)data;
                        else
                            Tabele = new TabeleBazaPodataka();
                    }
                    else
                    {
                        Tabele = new TabeleBazaPodataka();
                    }
                }
                catch
                {
                    Tabele = new TabeleBazaPodataka();
                }
            }

            public bool SacuvajPromene()
            {
                try
                {
                    XmlSerializer serializer =
                        new XmlSerializer(typeof(TabeleBazaPodataka));

                    StreamWriter sw =
                        new StreamWriter("podaci.xml");

                    serializer.Serialize(sw, Tabele);
                    sw.Close();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }

}
