namespace CafeteriaJam.Logica
{
    /// <summary>
    /// Todos los números del balance en un solo lugar.
    /// Si game design cambia un valor, se cambia aquí y nada más.
    /// </summary>
    public static class ReglasJuego
    {
        // --- Sanidad ---
        public const int SanidadInicial = 50;
        public const int SanidadMaxima = 100;
        public const int SanidadMinima = 0;           // llegar aquí = game over inmediato

        public const int UmbralSanidadAlta = 70;      // alta si sanidad >= 70
        public const int UmbralSanidadCritica = 30;   // crítica si sanidad < 30

        // Los efectos NO se acumulan: cada acción aplica solo su propio valor.
        public const int SanidadOrdenExitosa = 20;    // +20
        public const int SanidadCesionRara = 5;       // -5 por cada vez que cedes al cliente raro
        public const int SanidadOrdenMacabra = 40;    // -40 al entregar la atrocidad
        public const int SanidadErrorPreparacion = 5; // -5, y además no se cobra
        public const int SanidadRechazo = 0;          // rechazar no mueve la sanidad (por ahora)

        // --- Dinero ---
        public const int Cuota = 100;
        public const int PagoNormal = 30;
        public const int PagoRaro = 40;
        public const int PagoMacabro = 100;

        // PENDIENTE de confirmar con game design: cuánto se pierde por cada "más crema" que cedes.
        public const int DescuentoPorCesionRara = 10;
    }
}