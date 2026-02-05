using Domain.Enumeracije;
using Domain.Modeli;
namespace Domain.Servisi
{
    public interface ILoggerServis
    {
        void Evidentiraj(TipEvidencije tip, string poruka);
        void Upisi(LogZapis zapis);
    }
}
