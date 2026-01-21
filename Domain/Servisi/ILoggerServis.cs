using Domain.Enumeracije;

namespace Domain.Servisi
{
    public interface ILoggerServis
    {
        void Evidentiraj(TipEvidencije tip, string poruka);
    }
}
