using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;
using System;
using System.IO;

namespace Services.LoggerServisi
{
    public class LoggerServis : ILoggerServis
    {
        private readonly string putanja = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");

        public void Evidentiraj(TipEvidencije tip, string poruka)
        {
            Upisi(new LogZapis(tip, poruka));
        }

        public void Upisi(LogZapis zapis)
        {
            Console.WriteLine(zapis.ToString());
            File.AppendAllText(putanja, zapis.ToString() + Environment.NewLine);
        }
    }
}