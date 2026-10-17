namespace CafeteriaJam.Logica
{
    /// <summary>Cruza sanidad final contra dinero/cuota y devuelve uno de los 4 finales.</summary>
    public static class FinalesEvaluator
    {
        public static TipoFinal Evaluar(int sanidad, int dinero, int cuota)
        {
            // Colapso mental: siempre Desecho Desechado, sin importar el dinero.
            if (sanidad <= ReglasJuego.SanidadMinima)
                return TipoFinal.DesechoDesechado;

            bool sanidadAlta = sanidad >= ReglasJuego.UmbralSanidadAlta;
            bool cuotaCumplida = dinero >= cuota;

            if (sanidadAlta)
                return cuotaCumplida ? TipoFinal.EquilibrioAsertivo : TipoFinal.RenunciaDigna;

            return cuotaCumplida ? TipoFinal.SocioEnLasSombras : TipoFinal.DesechoDesechado;
        }
    }
}