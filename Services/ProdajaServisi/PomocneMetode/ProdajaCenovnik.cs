using Domain.Enumeracije;

namespace Services.ProdajaServisi.PomocneMetode
{
    internal class ProdajaCenovnik
    {
        public decimal IzracunajCenu(TipProdaje tipProdaje)
        {
            decimal bazna = 10m;
            return tipProdaje == TipProdaje.Diskont ? bazna * 0.85m : bazna;
        }
    }
}
