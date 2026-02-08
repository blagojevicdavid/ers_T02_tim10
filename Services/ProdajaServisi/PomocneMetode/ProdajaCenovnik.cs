using Domain.Enumeracije;

namespace Services.ProdajaServisi.PomocneMetode
{
    internal class ProdajaCenovnik
    {
        public decimal IzracunajCenu(TipProdaje tipProdaje)
        {
            decimal bazna = 10m;
            if (tipProdaje == TipProdaje.Diskont)
            {
                return bazna * 0.85m;
            }
            else
            {
                return bazna;
            }

        }
    }
}
