using System;

namespace CafeteriaJam.Logica
{
    /// <summary>
    /// Barra de sanidad. Siempre entre 0 y 100. Al llegar a 0 dispara GameOver una sola vez
    /// y a partir de ahí ignora cualquier cambio.
    /// </summary>
    public class SanidadManager
    {
        public int Sanidad { get; private set; }

        /// <summary>(valorAnterior, valorNuevo). Para que la UI mueva la barra.</summary>
        public event Action<int, int> SanidadCambio;

        /// <summary>Se dispara cuando la sanidad toca 0.</summary>
        public event Action GameOver;

        public SanidadManager(int sanidadInicial = ReglasJuego.SanidadInicial)
        {
            Sanidad = Limitar(sanidadInicial);
        }

        public bool EsGameOver { get { return Sanidad <= ReglasJuego.SanidadMinima; } }

        // --- Métodos que pidió la jefa de equipo ---
        public void AplicarOrdenExitosa()     { AplicarCambio(+ReglasJuego.SanidadOrdenExitosa); }
        public void AplicarCesionRara()       { AplicarCambio(-ReglasJuego.SanidadCesionRara); }
        public void AplicarOrdenMacabra()     { AplicarCambio(-ReglasJuego.SanidadOrdenMacabra); }
        public void AplicarErrorPreparacion() { AplicarCambio(-ReglasJuego.SanidadErrorPreparacion); }

        /// <summary>Aplica cualquier cambio (positivo o negativo). Lo usa TurnoModel con los valores de cada Pedido.</summary>
        public void AplicarCambio(int delta)
        {
            if (EsGameOver || delta == 0) return;

            int anterior = Sanidad;
            Sanidad = Limitar(Sanidad + delta);

            if (Sanidad != anterior && SanidadCambio != null)
                SanidadCambio(anterior, Sanidad);

            if (EsGameOver && GameOver != null)
                GameOver();
        }

        // --- Umbrales de percepción ---
        public bool EsSanidadAlta()    { return Sanidad >= ReglasJuego.UmbralSanidadAlta; }
        public bool EsSanidadCritica() { return Sanidad < ReglasJuego.UmbralSanidadCritica; }

        private static int Limitar(int valor)
        {
            if (valor < ReglasJuego.SanidadMinima) return ReglasJuego.SanidadMinima;
            if (valor > ReglasJuego.SanidadMaxima) return ReglasJuego.SanidadMaxima;
            return valor;
        }
    }
}