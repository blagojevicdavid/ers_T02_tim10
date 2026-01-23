using System;
using Domain.Enumeracije;
using Domain.Servisi;

namespace Services.LoggerServisi
{
    public class LoggerServis : ILoggerServis
    {
        public void Evidentiraj(TipEvidencije tip, string poruka)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {tip}: {poruka}");
        }
    }
}
